using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Web.Models;
using CarRental.Web.Services;

namespace CarRental.Web.Controllers;

// This controller handles rental requests
public class RentalsController : Controller
{
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;
    private readonly PdfService _pdfService;

    // Constructor - receives database context and services
    public RentalsController(AppDbContext context, EmailService emailService, PdfService pdfService)
    {
        _context = context;
        _emailService = emailService;
        _pdfService = pdfService;
    }

    // This action displays the form to request a rental
    [HttpGet]
    public async Task<IActionResult> RequestRental(int vehicleId)
    {
        // Check if user is logged in
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get the vehicle
        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == vehicleId);

        if (vehicle == null || vehicle.Status != VehicleStatus.Available)
        {
            return NotFound();
        }

        // Create view model with default dates
        var viewModel = new RentalRequestViewModel
        {
            VehicleId = vehicleId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7)
        };

        ViewBag.Vehicle = vehicle;
        return View(viewModel);
    }

    // This action processes the rental request
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestRental(RentalRequestViewModel model)
    {
        // Check if user is logged in
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get vehicle once for validation errors
        var vehicleForValidation = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == model.VehicleId);

        if (!ModelState.IsValid)
        {
            ViewBag.Vehicle = vehicleForValidation;
            return View(model);
        }

        // Validate dates
        if (model.StartDate < DateTime.Now.Date)
        {
            ModelState.AddModelError("StartDate", "Start date cannot be in the past.");
            ViewBag.Vehicle = vehicleForValidation;
            return View(model);
        }

        if (model.EndDate <= model.StartDate)
        {
            ModelState.AddModelError("EndDate", "End date must be after start date.");
            ViewBag.Vehicle = vehicleForValidation;
            return View(model);
        }

        // Get client
        var client = await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId.Value);

        if (client == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get vehicle
        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == model.VehicleId);

        if (vehicle == null || vehicle.Status != VehicleStatus.Available)
        {
            ModelState.AddModelError("", "Vehicle is not available.");
            ViewBag.Vehicle = vehicle;
            return View(model);
        }

        // Get first employee (for processing)
        var employee = await _context.Employees.FirstOrDefaultAsync();
        if (employee == null)
        {
            ModelState.AddModelError("", "No employees available to process rental.");
            ViewBag.Vehicle = vehicle;
            return View(model);
        }

        // Calculate rental details
        int numberOfDays = (model.EndDate - model.StartDate).Days + 1;
        decimal dailyRate = vehicle.VehicleType?.DailyRate ?? 0;
        decimal totalAmount = dailyRate * numberOfDays;

        // Create rental
        var rental = new Rental
        {
            ClientId = client.Id,
            VehicleId = vehicle.Id,
            EmployeeId = employee.Id,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            NumberOfDays = numberOfDays,
            DailyRate = dailyRate,
            TotalAmount = totalAmount,
            Status = RentalStatus.Pending,
            CreatedAt = DateTime.Now,
            Notes = model.Notes
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        // Send email confirmation (async, don't wait)
        _ = Task.Run(async () =>
        {
            await _emailService.SendRentalConfirmationAsync(
                client.User?.Email ?? "",
                $"{client.FirstName} {client.LastName}",
                rental.Id,
                $"{vehicle.Make} {vehicle.Model} ({vehicle.LicensePlate})",
                rental.StartDate,
                rental.EndDate,
                rental.TotalAmount
            );
        });

        return RedirectToAction("Confirmation", new { id = rental.Id });
    }

    // This action displays the rental confirmation page
    public async Task<IActionResult> Confirmation(int id)
    {
        // Check if user is logged in
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        var rental = await _context.Rentals
            .Include(r => r.Client)
            .ThenInclude(c => c!.User)
            .Include(r => r.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental == null || rental.Client?.UserId != userId.Value)
        {
            return NotFound();
        }

        return View(rental);
    }

    // This action downloads the PDF reservation
    public async Task<IActionResult> DownloadPdf(int id)
    {
        // Check if user is logged in
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        var rental = await _context.Rentals
            .Include(r => r.Client)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental == null || rental.Client?.UserId != userId.Value)
        {
            return NotFound();
        }

        // Generate PDF
        byte[] pdfBytes = _pdfService.GenerateReservationPdf(id);

        return File(pdfBytes, "application/pdf", $"Reservation_{id}.pdf");
    }

    // This action displays the client dashboard with their rentals
    public async Task<IActionResult> Dashboard()
    {
        // Check if user is logged in
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get client
        var client = await _context.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId.Value);

        if (client == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // Get all rentals for this client
        var rentals = await _context.Rentals
            .Include(r => r.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .Where(r => r.ClientId == client.Id)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        ViewBag.ClientName = $"{client.FirstName} {client.LastName}";
        ViewBag.IsLoggedIn = true;
        ViewBag.Username = HttpContext.Session.GetString("Username");
        return View(rentals);
    }
}


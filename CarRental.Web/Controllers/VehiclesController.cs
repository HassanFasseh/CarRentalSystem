using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Data;
using CarRental.Domain.Enums;
using CarRental.Web.Models;

namespace CarRental.Web.Controllers;

// This controller handles browsing and searching vehicles
public class VehiclesController : Controller
{
    private readonly AppDbContext _context;

    // Constructor - receives database context
    public VehiclesController(AppDbContext context)
    {
        _context = context;
    }

    // This action displays available vehicles with search and pagination
    public async Task<IActionResult> Index(string searchTerm, int? vehicleTypeId, int page = 1)
    {
        // Get all vehicle types for the filter dropdown
        var vehicleTypes = await _context.VehicleTypes.ToListAsync();

        // Start with all available vehicles
        var query = _context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.Status == VehicleStatus.Available);

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(v => 
                v.Make.Contains(searchTerm) || 
                v.Model.Contains(searchTerm) || 
                v.LicensePlate.Contains(searchTerm));
        }

        // Apply vehicle type filter if provided
        if (vehicleTypeId.HasValue)
        {
            query = query.Where(v => v.VehicleTypeId == vehicleTypeId.Value);
        }

        // Get total count for pagination
        int totalCount = await query.CountAsync();
        int pageSize = 6; // Show 6 vehicles per page
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Apply pagination
        var vehicles = await query
            .OrderBy(v => v.Make)
            .ThenBy(v => v.Model)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Create view model
        var viewModel = new VehicleSearchViewModel
        {
            SearchTerm = searchTerm,
            VehicleTypeId = vehicleTypeId,
            Vehicles = vehicles,
            VehicleTypes = vehicleTypes,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(viewModel);
    }

    // This action displays details of a specific vehicle
    public async Task<IActionResult> Details(int id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehicle == null)
        {
            return NotFound();
        }

        ViewBag.IsLoggedIn = HttpContext.Session.GetInt32("UserId") != null;
        return View(vehicle);
    }
}


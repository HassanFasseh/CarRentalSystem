using Microsoft.AspNetCore.Mvc;
using CarRental.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Controllers;

// This is the main controller for the home page
public class HomeController : Controller
{
    private readonly AppDbContext _context;

    // Constructor - receives database context
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    // This action displays the home page
    // It shows featured vehicles and general information
    public async Task<IActionResult> Index()
    {
        // Get featured vehicles (first 6 available vehicles)
        var featuredVehicles = await _context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.Status == CarRental.Domain.Enums.VehicleStatus.Available)
            .Take(6)
            .ToListAsync();

        ViewBag.FeaturedVehicles = featuredVehicles;
        
        // Set login status for layout
        ViewBag.IsLoggedIn = HttpContext.Session.GetInt32("UserId") != null;
        ViewBag.Username = HttpContext.Session.GetString("Username");

        return View();
    }

    // This action displays the privacy page
    public IActionResult Privacy()
    {
        return View();
    }

    // This action displays error information
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}

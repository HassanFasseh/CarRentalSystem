using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using CarRental.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Web.Controllers;

// This controller handles user registration and login
// It provides simple authentication without using ASP.NET Core Identity
public class AccountController : Controller
{
    private readonly AppDbContext _context;

    // Constructor - receives database context
    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    // This action displays the registration form
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // This action processes the registration form
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        // Check if model is valid
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if username already exists
        bool usernameExists = await _context.Users.AnyAsync(u => u.Username == model.Username);
        if (usernameExists)
        {
            ModelState.AddModelError("Username", "Username already exists.");
            return View(model);
        }

        // Check if email already exists
        bool emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
        if (emailExists)
        {
            ModelState.AddModelError("Email", "Email already exists.");
            return View(model);
        }

        // Create new user
        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            PasswordHash = HashPassword(model.Password),
            Role = UserRole.Client,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Create client record
        var client = new Client
        {
            UserId = user.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Address = model.Address,
            DriverLicenseNumber = model.DriverLicenseNumber,
            CreatedAt = DateTime.Now
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        // Store user ID in session
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);

        return RedirectToAction("Index", "Home");
    }

    // This action displays the login form
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // This action processes the login form
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Find user by username
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == model.Username && u.IsActive);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        // Verify password
        string hashedPassword = HashPassword(model.Password);
        if (user.PasswordHash != hashedPassword)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        // Store user ID in session
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);

        return RedirectToAction("Index", "Home");
    }

    // This action logs out the user
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    // This method hashes a password using SHA256
    private static string HashPassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            stringBuilder.Append(hashBytes[i].ToString("x2"));
        }
        
        return stringBuilder.ToString();
    }
}


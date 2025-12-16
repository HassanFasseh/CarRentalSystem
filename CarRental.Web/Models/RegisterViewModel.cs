using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Models;

// This view model is used for user registration
// It contains all the fields needed to create a new user account
public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, ErrorMessage = "Password must be at least 6 characters", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [Display(Name = "Address")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Driver license number is required")]
    [Display(Name = "Driver License Number")]
    public string DriverLicenseNumber { get; set; } = string.Empty;
}


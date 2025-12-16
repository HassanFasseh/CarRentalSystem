using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

// This class represents a user in the system
// Users can be either Admin or Client
public class User
{
    // Unique identifier for the user
    public int Id { get; set; }
    
    // Username for login
    public string Username { get; set; } = string.Empty;
    
    // Email address of the user
    public string Email { get; set; } = string.Empty;
    
    // Hashed password stored in database
    public string PasswordHash { get; set; } = string.Empty;
    
    // Role of the user (Admin or Client)
    public UserRole Role { get; set; }
    
    // Date when the user account was created
    public DateTime CreatedAt { get; set; }
    
    // Whether the user account is active
    public bool IsActive { get; set; }
}


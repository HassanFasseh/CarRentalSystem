namespace CarRental.Domain.Entities;

// This class represents a client who can rent vehicles
// A client is linked to a User account for authentication
public class Client
{
    // Unique identifier for the client
    public int Id { get; set; }
    
    // Reference to the User account
    public int UserId { get; set; }
    
    // Navigation property to the User
    public User? User { get; set; }
    
    // First name of the client
    public string FirstName { get; set; } = string.Empty;
    
    // Last name of the client
    public string LastName { get; set; } = string.Empty;
    
    // Phone number of the client
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Address of the client
    public string Address { get; set; } = string.Empty;
    
    // Driver's license number
    public string DriverLicenseNumber { get; set; } = string.Empty;
    
    // Date when the client record was created
    public DateTime CreatedAt { get; set; }
}


namespace CarRental.Domain.Enums;

// This enum defines the different roles a user can have in the system
public enum UserRole
{
    // Admin role - can access back office and manage everything
    Admin = 1,
    
    // Client role - can browse vehicles and make rental requests
    Client = 2
}


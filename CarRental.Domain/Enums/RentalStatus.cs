namespace CarRental.Domain.Enums;

// This enum defines the different states a rental can be in
public enum RentalStatus
{
    // Rental request is pending approval
    Pending = 1,
    
    // Rental is active and vehicle is currently rented
    Active = 2,
    
    // Rental has been completed successfully
    Completed = 3,
    
    // Rental was cancelled
    Cancelled = 4,
    
    // Rental request was denied by admin
    Denied = 5
}


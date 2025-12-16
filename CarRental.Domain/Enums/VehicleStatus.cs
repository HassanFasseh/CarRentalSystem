namespace CarRental.Domain.Enums;

// This enum defines the different states a vehicle can be in
public enum VehicleStatus
{
    // Vehicle is available for rental
    Available = 1,
    
    // Vehicle is currently rented
    Rented = 2,
    
    // Vehicle is in maintenance
    Maintenance = 3
}


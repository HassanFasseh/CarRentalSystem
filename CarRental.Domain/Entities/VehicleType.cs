namespace CarRental.Domain.Entities;

// This class represents a type or category of vehicle
// Examples: Sedan, SUV, Truck, etc.
public class VehicleType
{
    // Unique identifier for the vehicle type
    public int Id { get; set; }
    
    // Name of the vehicle type (e.g., "Sedan", "SUV")
    public string Name { get; set; } = string.Empty;
    
    // Description of the vehicle type
    public string Description { get; set; } = string.Empty;
    
    // Daily rental rate for this vehicle type
    public decimal DailyRate { get; set; }
}


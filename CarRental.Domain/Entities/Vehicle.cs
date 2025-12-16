using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

// This class represents a vehicle that can be rented
public class Vehicle
{
    // Unique identifier for the vehicle
    public int Id { get; set; }
    
    // Reference to the vehicle type
    public int VehicleTypeId { get; set; }
    
    // Navigation property to the VehicleType
    public VehicleType? VehicleType { get; set; }
    
    // License plate number of the vehicle
    public string LicensePlate { get; set; } = string.Empty;
    
    // Make of the vehicle (e.g., "Toyota", "Ford")
    public string Make { get; set; } = string.Empty;
    
    // Model of the vehicle (e.g., "Camry", "Focus")
    public string Model { get; set; } = string.Empty;
    
    // Year the vehicle was manufactured
    public int Year { get; set; }
    
    // Color of the vehicle
    public string Color { get; set; } = string.Empty;
    
    // Current status of the vehicle (Available, Rented, Maintenance)
    public VehicleStatus Status { get; set; }
    
    // Mileage of the vehicle
    public int Mileage { get; set; }
    
    // Date when the vehicle was last serviced
    public DateTime? LastServiceDate { get; set; }
    
    // Date when the vehicle was added to the system
    public DateTime CreatedAt { get; set; }
}


using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

// This class represents a rental transaction
// A rental connects a client, vehicle, employee, and payment
public class Rental
{
    // Unique identifier for the rental
    public int Id { get; set; }
    
    // Reference to the client who is renting
    public int ClientId { get; set; }
    
    // Navigation property to the Client
    public Client? Client { get; set; }
    
    // Reference to the vehicle being rented
    public int VehicleId { get; set; }
    
    // Navigation property to the Vehicle
    public Vehicle? Vehicle { get; set; }
    
    // Reference to the employee who processed the rental
    public int EmployeeId { get; set; }
    
    // Navigation property to the Employee
    public Employee? Employee { get; set; }
    
    // Date when the rental starts
    public DateTime StartDate { get; set; }
    
    // Date when the rental ends
    public DateTime EndDate { get; set; }
    
    // Total number of days for the rental
    public int NumberOfDays { get; set; }
    
    // Daily rate at the time of rental
    public decimal DailyRate { get; set; }
    
    // Total amount for the rental
    public decimal TotalAmount { get; set; }
    
    // Current status of the rental (Pending, Active, Completed, Cancelled)
    public RentalStatus Status { get; set; }
    
    // Date when the rental was created
    public DateTime CreatedAt { get; set; }
    
    // Additional notes about the rental
    public string? Notes { get; set; }
}


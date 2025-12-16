namespace CarRental.Domain.Entities;

// This class represents an employee who manages rentals
// An employee is linked to a User account for authentication
public class Employee
{
    // Unique identifier for the employee
    public int Id { get; set; }
    
    // Reference to the User account
    public int UserId { get; set; }
    
    // Navigation property to the User
    public User? User { get; set; }
    
    // First name of the employee
    public string FirstName { get; set; } = string.Empty;
    
    // Last name of the employee
    public string LastName { get; set; } = string.Empty;
    
    // Employee ID or badge number
    public string EmployeeNumber { get; set; } = string.Empty;
    
    // Phone number of the employee
    public string PhoneNumber { get; set; } = string.Empty;
    
    // Date when the employee record was created
    public DateTime CreatedAt { get; set; }
}


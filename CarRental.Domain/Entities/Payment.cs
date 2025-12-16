using CarRental.Domain.Enums;

namespace CarRental.Domain.Entities;

// This class represents a payment for a rental
// Each rental can have one payment
public class Payment
{
    // Unique identifier for the payment
    public int Id { get; set; }
    
    // Reference to the rental this payment is for
    public int RentalId { get; set; }
    
    // Navigation property to the Rental
    public Rental? Rental { get; set; }
    
    // Amount of the payment
    public decimal Amount { get; set; }
    
    // Status of the payment (Pending, Paid, Failed)
    public PaymentStatus Status { get; set; }
    
    // Date when the payment was made
    public DateTime PaymentDate { get; set; }
    
    // Payment method (e.g., "Credit Card", "Cash", "Bank Transfer")
    public string PaymentMethod { get; set; } = string.Empty;
    
    // Transaction reference number if available
    public string? TransactionReference { get; set; }
    
    // Date when the payment record was created
    public DateTime CreatedAt { get; set; }
}


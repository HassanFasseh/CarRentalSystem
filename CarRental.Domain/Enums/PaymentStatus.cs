namespace CarRental.Domain.Enums;

// This enum defines the different states a payment can be in
public enum PaymentStatus
{
    // Payment is pending
    Pending = 1,
    
    // Payment has been completed successfully
    Paid = 2,
    
    // Payment failed
    Failed = 3
}


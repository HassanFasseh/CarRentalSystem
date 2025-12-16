using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;

namespace CarRental.Data;

// This is the main database context class
// It connects our entities to the MySQL/MariaDB database (XAMPP)
public class AppDbContext : DbContext
{
    // Constructor that takes DbContextOptions
    // This allows us to configure the database connection
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet for Users table
    // This represents all users in the database
    public DbSet<User> Users { get; set; }

    // DbSet for Clients table
    // This represents all clients in the database
    public DbSet<Client> Clients { get; set; }

    // DbSet for Employees table
    // This represents all employees in the database
    public DbSet<Employee> Employees { get; set; }

    // DbSet for VehicleTypes table
    // This represents all vehicle types in the database
    public DbSet<VehicleType> VehicleTypes { get; set; }

    // DbSet for Vehicles table
    // This represents all vehicles in the database
    public DbSet<Vehicle> Vehicles { get; set; }

    // DbSet for Rentals table
    // This represents all rentals in the database
    public DbSet<Rental> Rentals { get; set; }

    // DbSet for Payments table
    // This represents all payments in the database
    public DbSet<Payment> Payments { get; set; }

    // This method is called when the model is being created
    // We use it to configure relationships between entities
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the relationship between Client and User
        // One User can have one Client record
        modelBuilder.Entity<Client>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Employee and User
        // One User can have one Employee record
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Vehicle and VehicleType
        // One VehicleType can have many Vehicles
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.VehicleType)
            .WithMany()
            .HasForeignKey(v => v.VehicleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Rental and Client
        // One Client can have many Rentals
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Client)
            .WithMany()
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Rental and Vehicle
        // One Vehicle can have many Rentals
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Vehicle)
            .WithMany()
            .HasForeignKey(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Rental and Employee
        // One Employee can have many Rentals
        modelBuilder.Entity<Rental>()
            .HasOne(r => r.Employee)
            .WithMany()
            .HasForeignKey(r => r.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship between Payment and Rental
        // One Rental can have one Payment
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Rental)
            .WithMany()
            .HasForeignKey(p => p.RentalId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure decimal precision for VehicleType.DailyRate
        // This ensures money values are stored with proper precision (18 digits, 2 decimal places)
        modelBuilder.Entity<VehicleType>()
            .Property(vt => vt.DailyRate)
            .HasPrecision(18, 2);

        // Configure decimal precision for Rental.DailyRate
        modelBuilder.Entity<Rental>()
            .Property(r => r.DailyRate)
            .HasPrecision(18, 2);

        // Configure decimal precision for Rental.TotalAmount
        modelBuilder.Entity<Rental>()
            .Property(r => r.TotalAmount)
            .HasPrecision(18, 2);

        // Configure decimal precision for Payment.Amount
        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);
    }
}


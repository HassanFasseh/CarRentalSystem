using Microsoft.EntityFrameworkCore;
using CarRental.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CarRental.BackOffice.Services;

// This service handles database operations for the WPF application
// It initializes the database and provides access to the context
public class DatabaseService
{
    private AppDbContext? _context;

    // This method creates a new database context
    // Each call creates a new context to avoid disposal issues
    public AppDbContext GetContext()
    {
        // Create options for the database context
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(DbConnectionHelper.GetConnectionString(), 
            new MySqlServerVersion(new Version(8, 0, 21)));
        
        // Always create a new context to avoid disposal issues
        return new AppDbContext(optionsBuilder.Options);
    }

    // This method ensures the database is created and migrated
    // It also seeds initial data if the database is empty
    public void InitializeDatabase()
    {
        // Create a new context for initialization (don't use the cached one)
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(DbConnectionHelper.GetConnectionString(), 
            new MySqlServerVersion(new Version(8, 0, 21)));
        
        using var context = new AppDbContext(optionsBuilder.Options);
        
        // Apply any pending migrations
        // This creates the database if it doesn't exist
        try
        {
            context.Database.Migrate();
        }
        catch
        {
            // If migrations fail, try to create the database
            context.Database.EnsureCreated();
        }
        
        // Seed the database with initial data
        SeedData.SeedDatabase(context);
    }

    // This method disposes the context when done
    // Note: Since we create new contexts each time, this is mainly for cleanup
    public void Dispose()
    {
        _context?.Dispose();
        _context = null;
    }
}


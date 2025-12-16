using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CarRental.Data;

// This factory is used by Entity Framework Core tools during design time
// It allows migrations to be created even though this is a class library
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    // This method creates a DbContext instance for design-time operations
    // It's called when you run migrations or other EF Core commands
    public AppDbContext CreateDbContext(string[] args)
    {
        // Create options builder
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use the connection string from our helper for MySQL/MariaDB
        optionsBuilder.UseMySql(DbConnectionHelper.GetConnectionString(), 
            new MySqlServerVersion(new Version(8, 0, 21)));
        
        // Return a new instance of the context
        return new AppDbContext(optionsBuilder.Options);
    }
}


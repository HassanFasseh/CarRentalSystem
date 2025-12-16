namespace CarRental.Data;

// This helper class provides the database connection string
// We use MySQL/MariaDB from XAMPP for this project
public static class DbConnectionHelper
{
    // This method returns the connection string for MySQL/MariaDB
    // XAMPP includes MySQL/MariaDB which runs on localhost:3306 by default
    // The database will be created automatically when you run migrations
    public static string GetConnectionString()
    {
        // Connection string for MySQL/MariaDB (XAMPP)
        // Default XAMPP MySQL settings:
        // - Server: localhost (or 127.0.0.1)
        // - Port: 3306
        // - Username: root (default)
        // - Password: (empty by default, change if you set a password)
        // - Database: CarRentalDb (will be created automatically)
        return "Server=localhost;Port=3307;Database=CarRentalDb;User=root;Password=;CharSet=utf8mb4;";
    }

    // Alternative connection string if you set a password for MySQL root user
    // Uncomment and modify if you have set a password for the root user
    // public static string GetConnectionString()
    // {
    //     return "Server=localhost;Port=3306;Database=CarRentalDb;User=root;Password=yourpassword;CharSet=utf8mb4;";
    // }
}

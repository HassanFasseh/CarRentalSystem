using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using System.Security.Cryptography;
using System.Text;

namespace CarRental.Data;

// This class contains methods to seed the database with initial data
// Seed data is sample data that helps you test the application
public static class SeedData
{
    // This method seeds the database with initial data
    // It should be called after the database is created
    public static void SeedDatabase(AppDbContext context)
    {
        // Make sure the database exists
        context.Database.EnsureCreated();

        // Check if data already exists to avoid duplicates
        if (context.Users.Any())
        {
            return; // Database already has data, skip seeding
        }

        // Seed Users
        SeedUsers(context);

        // Seed Vehicle Types
        SeedVehicleTypes(context);

        // Seed Vehicles
        SeedVehicles(context);

        // Seed Clients
        SeedClients(context);

        // Seed Employees
        SeedEmployees(context);

        // Save all changes to the database
        context.SaveChanges();
    }

    // This method creates sample users
    // We create an admin user and a few client users
    private static void SeedUsers(AppDbContext context)
    {
        // Create admin user
        // Password: Admin123! (hashed)
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@carrental.com",
            PasswordHash = HashPassword("Admin123!"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.Now,
            IsActive = true
        };
        context.Users.Add(adminUser);

        // Create client user 1
        var clientUser1 = new User
        {
            Username = "john.doe",
            Email = "john.doe@email.com",
            PasswordHash = HashPassword("Client123!"),
            Role = UserRole.Client,
            CreatedAt = DateTime.Now,
            IsActive = true
        };
        context.Users.Add(clientUser1);

        // Create client user 2
        var clientUser2 = new User
        {
            Username = "jane.smith",
            Email = "jane.smith@email.com",
            PasswordHash = HashPassword("Client123!"),
            Role = UserRole.Client,
            CreatedAt = DateTime.Now,
            IsActive = true
        };
        context.Users.Add(clientUser2);

        // Save users first so we can reference them
        context.SaveChanges();
    }

    // This method creates sample vehicle types
    private static void SeedVehicleTypes(AppDbContext context)
    {
        var vehicleTypes = new List<VehicleType>
        {
            new VehicleType
            {
                Name = "Sedan",
                Description = "Comfortable 4-door sedan, perfect for city driving",
                DailyRate = 50.00m
            },
            new VehicleType
            {
                Name = "SUV",
                Description = "Spacious SUV, great for families and long trips",
                DailyRate = 80.00m
            },
            new VehicleType
            {
                Name = "Sports Car",
                Description = "High-performance sports car for the ultimate driving experience",
                DailyRate = 150.00m
            },
            new VehicleType
            {
                Name = "Economy",
                Description = "Fuel-efficient compact car, perfect for budget-conscious renters",
                DailyRate = 35.00m
            },
            new VehicleType
            {
                Name = "Luxury",
                Description = "Premium luxury vehicle with all the amenities",
                DailyRate = 200.00m
            }
        };

        context.VehicleTypes.AddRange(vehicleTypes);
        context.SaveChanges();
    }

    // This method creates sample vehicles
    private static void SeedVehicles(AppDbContext context)
    {
        // Get vehicle types that were just created
        var sedanType = context.VehicleTypes.FirstOrDefault(vt => vt.Name == "Sedan");
        var suvType = context.VehicleTypes.FirstOrDefault(vt => vt.Name == "SUV");
        var sportsType = context.VehicleTypes.FirstOrDefault(vt => vt.Name == "Sports Car");
        var economyType = context.VehicleTypes.FirstOrDefault(vt => vt.Name == "Economy");
        var luxuryType = context.VehicleTypes.FirstOrDefault(vt => vt.Name == "Luxury");

        var vehicles = new List<Vehicle>
        {
            // Sedans
            new Vehicle
            {
                VehicleTypeId = sedanType!.Id,
                LicensePlate = "ABC-123",
                Make = "Toyota",
                Model = "Camry",
                Year = 2022,
                Color = "Silver",
                Status = VehicleStatus.Available,
                Mileage = 15000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = sedanType.Id,
                LicensePlate = "DEF-456",
                Make = "Honda",
                Model = "Accord",
                Year = 2023,
                Color = "Black",
                Status = VehicleStatus.Available,
                Mileage = 8000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            // SUVs
            new Vehicle
            {
                VehicleTypeId = suvType!.Id,
                LicensePlate = "GHI-789",
                Make = "Ford",
                Model = "Explorer",
                Year = 2022,
                Color = "White",
                Status = VehicleStatus.Available,
                Mileage = 20000,
                LastServiceDate = DateTime.Now.AddMonths(-3),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = suvType.Id,
                LicensePlate = "JKL-012",
                Make = "Chevrolet",
                Model = "Tahoe",
                Year = 2023,
                Color = "Blue",
                Status = VehicleStatus.Rented,
                Mileage = 12000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            // Sports Cars
            new Vehicle
            {
                VehicleTypeId = sportsType!.Id,
                LicensePlate = "MNO-345",
                Make = "Porsche",
                Model = "911",
                Year = 2023,
                Color = "Red",
                Status = VehicleStatus.Available,
                Mileage = 5000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            // Economy
            new Vehicle
            {
                VehicleTypeId = economyType!.Id,
                LicensePlate = "PQR-678",
                Make = "Nissan",
                Model = "Versa",
                Year = 2022,
                Color = "Gray",
                Status = VehicleStatus.Available,
                Mileage = 18000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            // Luxury
            new Vehicle
            {
                VehicleTypeId = luxuryType!.Id,
                LicensePlate = "STU-901",
                Make = "Mercedes-Benz",
                Model = "S-Class",
                Year = 2023,
                Color = "Black",
                Status = VehicleStatus.Maintenance,
                Mileage = 3000,
                LastServiceDate = DateTime.Now.AddMonths(-6),
                CreatedAt = DateTime.Now
            },
            // More Sedans
            new Vehicle
            {
                VehicleTypeId = sedanType.Id,
                LicensePlate = "VWX-234",
                Make = "Nissan",
                Model = "Altima",
                Year = 2023,
                Color = "Blue",
                Status = VehicleStatus.Available,
                Mileage = 10000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = sedanType.Id,
                LicensePlate = "YZA-567",
                Make = "Hyundai",
                Model = "Elantra",
                Year = 2022,
                Color = "White",
                Status = VehicleStatus.Available,
                Mileage = 22000,
                LastServiceDate = DateTime.Now.AddMonths(-3),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = sedanType.Id,
                LicensePlate = "BCD-890",
                Make = "Mazda",
                Model = "Mazda6",
                Year = 2023,
                Color = "Red",
                Status = VehicleStatus.Available,
                Mileage = 7000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            // More SUVs
            new Vehicle
            {
                VehicleTypeId = suvType.Id,
                LicensePlate = "EFG-123",
                Make = "Toyota",
                Model = "RAV4",
                Year = 2023,
                Color = "Silver",
                Status = VehicleStatus.Available,
                Mileage = 9000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = suvType.Id,
                LicensePlate = "HIJ-456",
                Make = "Honda",
                Model = "CR-V",
                Year = 2022,
                Color = "Black",
                Status = VehicleStatus.Available,
                Mileage = 16000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = suvType.Id,
                LicensePlate = "KLM-789",
                Make = "Jeep",
                Model = "Grand Cherokee",
                Year = 2023,
                Color = "Gray",
                Status = VehicleStatus.Available,
                Mileage = 6000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            // More Economy Cars
            new Vehicle
            {
                VehicleTypeId = economyType.Id,
                LicensePlate = "NOP-012",
                Make = "Toyota",
                Model = "Corolla",
                Year = 2022,
                Color = "White",
                Status = VehicleStatus.Available,
                Mileage = 19000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = economyType.Id,
                LicensePlate = "QRS-345",
                Make = "Honda",
                Model = "Civic",
                Year = 2023,
                Color = "Blue",
                Status = VehicleStatus.Available,
                Mileage = 11000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = economyType.Id,
                LicensePlate = "TUV-678",
                Make = "Kia",
                Model = "Rio",
                Year = 2022,
                Color = "Red",
                Status = VehicleStatus.Available,
                Mileage = 25000,
                LastServiceDate = DateTime.Now.AddMonths(-3),
                CreatedAt = DateTime.Now
            },
            // More Sports Cars
            new Vehicle
            {
                VehicleTypeId = sportsType.Id,
                LicensePlate = "WXY-901",
                Make = "Chevrolet",
                Model = "Camaro",
                Year = 2023,
                Color = "Yellow",
                Status = VehicleStatus.Available,
                Mileage = 4000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = sportsType.Id,
                LicensePlate = "ZAB-234",
                Make = "Ford",
                Model = "Mustang",
                Year = 2022,
                Color = "Blue",
                Status = VehicleStatus.Available,
                Mileage = 13000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            // More Luxury Cars
            new Vehicle
            {
                VehicleTypeId = luxuryType.Id,
                LicensePlate = "CDE-567",
                Make = "BMW",
                Model = "7 Series",
                Year = 2023,
                Color = "Black",
                Status = VehicleStatus.Available,
                Mileage = 2000,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = luxuryType.Id,
                LicensePlate = "FGH-890",
                Make = "Audi",
                Model = "A8",
                Year = 2023,
                Color = "Silver",
                Status = VehicleStatus.Available,
                Mileage = 3500,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = luxuryType.Id,
                LicensePlate = "IJK-123",
                Make = "Lexus",
                Model = "LS",
                Year = 2022,
                Color = "White",
                Status = VehicleStatus.Available,
                Mileage = 8000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            // Additional variety
            new Vehicle
            {
                VehicleTypeId = sedanType.Id,
                LicensePlate = "LMN-456",
                Make = "Volkswagen",
                Model = "Passat",
                Year = 2022,
                Color = "Gray",
                Status = VehicleStatus.Available,
                Mileage = 17000,
                LastServiceDate = DateTime.Now.AddMonths(-2),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = suvType.Id,
                LicensePlate = "OPQ-789",
                Make = "Nissan",
                Model = "Pathfinder",
                Year = 2023,
                Color = "White",
                Status = VehicleStatus.Available,
                Mileage = 7500,
                LastServiceDate = DateTime.Now.AddMonths(-1),
                CreatedAt = DateTime.Now
            },
            new Vehicle
            {
                VehicleTypeId = economyType.Id,
                LicensePlate = "RST-012",
                Make = "Chevrolet",
                Model = "Spark",
                Year = 2022,
                Color = "Orange",
                Status = VehicleStatus.Available,
                Mileage = 21000,
                LastServiceDate = DateTime.Now.AddMonths(-3),
                CreatedAt = DateTime.Now
            }
        };

        context.Vehicles.AddRange(vehicles);
        context.SaveChanges();
    }

    // This method creates sample clients
    private static void SeedClients(AppDbContext context)
    {
        // Get users that were just created
        var clientUser1 = context.Users.FirstOrDefault(u => u.Username == "john.doe");
        var clientUser2 = context.Users.FirstOrDefault(u => u.Username == "jane.smith");

        var clients = new List<Client>
        {
            new Client
            {
                UserId = clientUser1!.Id,
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "555-0101",
                Address = "123 Main Street, City, State 12345",
                DriverLicenseNumber = "DL123456",
                CreatedAt = DateTime.Now
            },
            new Client
            {
                UserId = clientUser2!.Id,
                FirstName = "Jane",
                LastName = "Smith",
                PhoneNumber = "555-0102",
                Address = "456 Oak Avenue, City, State 12345",
                DriverLicenseNumber = "DL789012",
                CreatedAt = DateTime.Now
            }
        };

        context.Clients.AddRange(clients);
        context.SaveChanges();
    }

    // This method creates sample employees
    private static void SeedEmployees(AppDbContext context)
    {
        // Get admin user
        var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");

        var employees = new List<Employee>
        {
            new Employee
            {
                UserId = adminUser!.Id,
                FirstName = "Admin",
                LastName = "User",
                EmployeeNumber = "EMP001",
                PhoneNumber = "555-0001",
                CreatedAt = DateTime.Now
            }
        };

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }

    // This method hashes a password using SHA256
    // In a real application, you would use a more secure method like bcrypt
    // But for this student project, SHA256 is acceptable
    private static string HashPassword(string password)
    {
        // Convert password to bytes
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        
        // Create SHA256 hash
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        
        // Convert hash to string
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            stringBuilder.Append(hashBytes[i].ToString("x2"));
        }
        
        return stringBuilder.ToString();
    }
}


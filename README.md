# Car Rental System - Academic Project

A simple car rental management system built with .NET 8, MySQL/MariaDB (via XAMPP), and Entity Framework Core. This project demonstrates a basic layered architecture suitable for academic purposes.

## 📋 Project Overview

This solution consists of four projects:

- **CarRental.Domain** - Domain entities and enums (POCO classes)
- **CarRental.Data** - Database context, migrations, and seed data
- **CarRental.BackOffice** - WPF desktop application for administrators
- **CarRental.Web** - ASP.NET Core MVC web application for clients

## 🛠️ Technology Stack

- **.NET 8.0**
- **MySQL/MariaDB** (via XAMPP)
- **Entity Framework Core 8.0** (with Pomelo.EntityFrameworkCore.MySql)
- **WPF** (Windows Presentation Foundation)
- **ASP.NET Core MVC**
- **QuestPDF** - PDF generation
- **QRCoder** - QR code generation
- **CsvHelper** - CSV import/export
- **MailKit** - Email sending

## 📁 Solution Structure

```
CarRentalStudent/
├── CarRental.Domain/          # Domain entities and enums
│   ├── Entities/              # User, Client, Employee, Vehicle, etc.
│   └── Enums/                 # UserRole, RentalStatus, PaymentStatus, VehicleStatus
│
├── CarRental.Data/             # Data access layer
│   ├── AppDbContext.cs        # Database context
│   ├── DbConnectionHelper.cs  # Connection string helper
│   ├── SeedData.cs            # Initial data seeding
│   └── Migrations/            # Database migrations
│
├── CarRental.BackOffice/      # WPF Admin Application
│   ├── Windows/               # Login and edit windows
│   ├── Pages/                 # Dashboard and CRUD pages
│   └── Services/              # Database, CSV, PDF, QR code services
│
└── CarRental.Web/             # MVC Web Application
    ├── Controllers/           # Account, Vehicles, Rentals controllers
    ├── Views/                 # Razor views
    ├── Models/                # View models
    └── Services/              # Email and PDF services
```

## 🚀 Getting Started

### Prerequisites

1. **JetBrains Rider** (or Visual Studio 2022)
2. **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
3. **XAMPP** - [Download here](https://www.apachefriends.org/) (includes MySQL/MariaDB)
4. **Entity Framework Core Tools** (for migrations)

### Installation Steps

#### 1. Clone or Extract the Project

Open the solution file `CarRentalStudent.sln` in JetBrains Rider.

#### 2. Restore NuGet Packages

In Rider, right-click on the solution and select **Restore NuGet Packages**, or run:

```bash
dotnet restore
```

#### 3. Install Entity Framework Core Tools (if not already installed)

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```

#### 4. Start XAMPP and MySQL

1. **Install XAMPP** if you haven't already
2. **Start XAMPP Control Panel**
3. **Start MySQL** service (click the "Start" button next to MySQL)
4. MySQL will run on `localhost:3306` by default

**Note:** The default MySQL root user has no password. If you've set a password, you'll need to update the connection string (see step 5).

#### 5. Create the Database

The database will be created automatically when you run the application. However, if you want to create it manually:

**Option A: Automatic (Recommended)**
- Make sure XAMPP MySQL is running
- Just run the application - the database will be created and seeded automatically on first run

**Option B: Manual Migration**
```bash
cd CarRental.Data
dotnet ef database update --project CarRental.Data/CarRental.Data.csproj
```

#### 6. Configure Database Connection (Optional)

The default connection uses XAMPP's MySQL with these settings:
- **Server:** localhost
- **Port:** 3307
- **Username:** root
- **Password:** (empty by default)
- **Database:** CarRentalDb (created automatically)

If you've set a password for the MySQL root user, edit:

`CarRental.Data/DbConnectionHelper.cs`

Change the connection string to:
```csharp
return "Server=localhost;Port=3306;Database=CarRentalDb;User=root;Password=yourpassword;CharSet=utf8mb4;";
```

## 🔐 Default Accounts

### Admin Account

After running the application for the first time, the database will be seeded with a default admin account:

- **Username:** `admin`
- **Password:** `Admin123!`
- **Role:** Admin

### Test Client Accounts

Two test client accounts are also created:

1. **Username:** `john.doe`
   - **Password:** `Client123!`
   - **Email:** john.doe@email.com

2. **Username:** `jane.smith`
   - **Password:** `Client123!`
   - **Email:** jane.smith@email.com

## 🏃 Running the Applications

### Running the BackOffice (WPF Application)

1. In Rider, set `CarRental.BackOffice` as the startup project
2. Press **F5** or click the **Run** button
3. The login window will appear
4. Login with admin credentials: `admin` / `Admin123!`

### Running the Web Application

1. In Rider, set `CarRental.Web` as the startup project
2. Press **F5** or click the **Run** button
3. The application will open in your default browser (usually `https://localhost:5001` or `http://localhost:5000`)
4. You can browse vehicles without logging in, but you need to register/login to request rentals

## 📧 Email Configuration

Email functionality sends notifications for:
- Rental request confirmations
- Rental approval notifications
- Rental denial notifications

### Quick Setup (Gmail)

1. **Get Gmail App Password:**
   - Enable 2-Step Verification on your Google account
   - Go to: https://myaccount.google.com/apppasswords
   - Generate an App Password for "Mail"

2. **Configure Web Application:**
   - Edit `CarRental.Web/appsettings.json`
   - Fill in your Gmail settings:
   ```json
   {
     "EmailSettings": {
       "SmtpServer": "smtp.gmail.com",
       "SmtpPort": "587",
       "SmtpUsername": "your-email@gmail.com",
       "SmtpPassword": "your-16-character-app-password",
       "FromEmail": "your-email@gmail.com"
     }
   }
   ```

3. **Configure BackOffice:**
   - Copy `email.config.example` to `email.config` in the BackOffice output folder
   - Location: `CarRental.BackOffice/bin/Debug/net8.0-windows/email.config`
   - Fill in your SMTP settings (same as above)

**For detailed instructions and other email providers, see `EMAIL_SETUP.md`**

**Note:** If email is not configured, the application will still work but emails won't be sent (logged to debug output).

## ✨ Features

### BackOffice (WPF Application)

- ✅ **Login System** - Admin authentication
- ✅ **Dashboard** - Statistics overview (clients, vehicles, rentals, revenue)
- ✅ **Client Management** - CRUD operations for clients
- ✅ **Vehicle Management** - CRUD operations for vehicles
- ✅ **Vehicle Type Management** - Manage vehicle categories and pricing
- ✅ **Rental Management** - View and manage rental transactions
- ✅ **Payment Management** - Track payments
- ✅ **Availability Check** - Check vehicle availability for date ranges
- ✅ **Maintenance Alerts** - Find vehicles needing service
- ✅ **CSV Export** - Export data to CSV files
- ✅ **CSV Import** - Import data from CSV files
- ✅ **PDF Generation** - Generate reservation PDFs
- ✅ **QR Code Generation** - Generate QR codes for rentals

### Web Application (MVC)

- ✅ **User Registration** - Create new client accounts
- ✅ **User Login** - Simple session-based authentication
- ✅ **Browse Vehicles** - View available vehicles with search and filters
- ✅ **Vehicle Details** - Detailed vehicle information
- ✅ **Request Rental** - Submit rental requests
- ✅ **Email Confirmation** - Automatic email confirmation (if configured)
- ✅ **PDF Download** - Download reservation PDFs
- ✅ **Search & Pagination** - Search vehicles and paginate results

## 🗄️ Database Schema

The database (MySQL/MariaDB via XAMPP) includes the following tables:

- **Users** - User accounts (Admin/Client)
- **Clients** - Client information
- **Employees** - Employee information
- **VehicleTypes** - Vehicle categories
- **Vehicles** - Vehicle inventory
- **Rentals** - Rental transactions
- **Payments** - Payment records

**Note:** You can view and manage the database using phpMyAdmin (included with XAMPP) at `http://localhost/phpmyadmin`

## 📝 Code Style

This project follows a simple, student-friendly approach:

- ✅ Clear, descriptive variable names
- ✅ Extensive comments explaining logic
- ✅ Simple if/else statements (no complex LINQ tricks)
- ✅ Beginner-friendly patterns
- ✅ No enterprise architecture patterns
- ✅ No dependency injection magic
- ✅ Code-behind allowed in WPF (simpler approach)

## 🐛 Troubleshooting

### Database Connection Issues

If you get database connection errors:

1. **Check XAMPP MySQL is running:**
   - Open XAMPP Control Panel
   - Ensure MySQL service is started (green indicator)
   - If not running, click "Start" next to MySQL

2. **Verify MySQL port:**
   - Default MySQL port is 3306
   - If you changed it, update the connection string in `CarRental.Data/DbConnectionHelper.cs`

3. **Check MySQL credentials:**
   - Default username: `root`
   - Default password: (empty)
   - If you set a password, update the connection string

4. **Check connection string:**
   - Verify the connection string in `CarRental.Data/DbConnectionHelper.cs`
   - Ensure the database name matches: `CarRentalDb`
   - Format: `Server=localhost;Port=3306;Database=CarRentalDb;User=root;Password=;CharSet=utf8mb4;`

5. **Test MySQL connection:**
   - Open phpMyAdmin in XAMPP (usually at http://localhost/phpmyadmin)
   - Try to login with root user (no password by default)
   - If you can't connect, check XAMPP MySQL logs

### Migration Issues

If migrations fail:

1. Delete the database (if it exists)
2. Run: `dotnet ef database update --project CarRental.Data/CarRental.Data.csproj`
3. Or let the application create it automatically on first run

### Build Errors

If you encounter build errors:

1. **Restore packages:**
   ```bash
   dotnet restore
   ```

2. **Clean and rebuild:**
   ```bash
   dotnet clean
   dotnet build
   ```

3. **Check .NET version:**
   - Ensure you have .NET 8.0 SDK installed
   - Run: `dotnet --version` (should show 8.x.x)

## 📚 Additional Notes

### Password Hashing

The application uses SHA256 for password hashing. This is acceptable for an academic project but should be upgraded to bcrypt or ASP.NET Core Identity for production use.

### Session Management

The web application uses simple session-based authentication. Sessions expire after 30 minutes of inactivity.

### File Locations

- **CSV Exports:** Saved to `Documents` folder
- **PDF Downloads:** Generated on-demand in browser
- **QR Codes:** Can be saved from the BackOffice application

## 🎓 Academic Project Notes

This project is designed to be:

- **Simple and understandable** - No complex patterns
- **Well-commented** - Every method has explanatory comments
- **Student-made appearance** - Straightforward code structure
- **Easy to modify** - Clear separation of concerns

## 📄 License

This is an academic project for educational purposes.

## 👤 Author

Created as a final-year computer science academic project.

---

**Happy Coding! 🚗**


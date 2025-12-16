# Running Car Rental System in Visual Studio

This guide explains how to set up and run the Car Rental System project in **Visual Studio 2022**.

## 📋 Prerequisites

Before you begin, make sure you have:

1. **Visual Studio 2022** (Community, Professional, or Enterprise)
   - Download: [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
   - Make sure to install the **.NET desktop development** and **ASP.NET and web development** workloads

2. **.NET 8.0 SDK**
   - Usually included with Visual Studio 2022
   - Verify installation: Open **Developer Command Prompt** and run `dotnet --version` (should show 8.x.x)
   - If not installed: [Download .NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

3. **XAMPP** (for MySQL database)
   - Download: [XAMPP](https://www.apachefriends.org/)
   - Install and start MySQL service

4. **Entity Framework Core Tools** (for database migrations)
   - Will be installed automatically, or install manually (see step 3 below)

## 🚀 Step-by-Step Setup

### Step 1: Open the Solution in Visual Studio

1. **Launch Visual Studio 2022**

2. **Open the Solution:**
   - Click **File** → **Open** → **Project/Solution...**
   - Navigate to your project folder
   - Select `CarRentalStudent.sln`
   - Click **Open**

3. **Wait for Solution to Load:**
   - Visual Studio will automatically restore NuGet packages
   - Wait for the "Package restore completed" message in the Output window

### Step 2: Restore NuGet Packages (if needed)

If packages weren't restored automatically:

1. **Right-click on the solution** in Solution Explorer
2. Select **Restore NuGet Packages**
3. Wait for the restore to complete (check Output window)

**Alternative (using Package Manager Console):**
- Go to **Tools** → **NuGet Package Manager** → **Package Manager Console**
- Run: `dotnet restore`

### Step 3: Install Entity Framework Core Tools

1. **Open Developer Command Prompt:**
   - Press `Win + R`
   - Type `cmd` and press Enter
   - Or search for "Developer Command Prompt for VS 2022" in Start Menu

2. **Install EF Core Tools:**
   ```bash
   dotnet tool install --global dotnet-ef --version 8.0.0
   ```

3. **Verify Installation:**
   ```bash
   dotnet ef --version
   ```
   Should show version 8.0.0

### Step 4: Start XAMPP and MySQL

1. **Open XAMPP Control Panel**
   - Usually located at: `C:\xampp\xampp-control.exe`

2. **Start MySQL Service:**
   - Click the **Start** button next to MySQL
   - Wait for the status to turn **green**
   - MySQL is now running on `localhost:3306` (or `3307` if you changed the port)

3. **Verify MySQL is Running:**
   - The MySQL status should show "Running" in green
   - You can test by opening phpMyAdmin: `http://localhost/phpmyadmin`

### Step 5: Configure Database Connection (if needed)

The default connection uses:
- **Server:** localhost
- **Port:** 3307 (check your XAMPP MySQL port)
- **Username:** root
- **Password:** (empty by default)
- **Database:** CarRentalDb (created automatically)

**To change the connection string:**

1. In Solution Explorer, navigate to: `CarRental.Data` → `DbConnectionHelper.cs`
2. Double-click to open the file
3. Find the connection string and update if needed:
   ```csharp
   return "Server=localhost;Port=3307;Database=CarRentalDb;User=root;Password=yourpassword;CharSet=utf8mb4;";
   ```
4. **Save** the file (Ctrl + S)

### Step 6: Build the Solution

1. **Build the Solution:**
   - Go to **Build** → **Build Solution** (or press `Ctrl + Shift + B`)
   - Wait for the build to complete
   - Check the **Output** window for any errors

2. **Fix any Build Errors:**
   - If you see errors, check the Error List window (View → Error List)
   - Common issues:
     - Missing NuGet packages → Restore packages again
     - .NET 8.0 not found → Install .NET 8.0 SDK
     - MySQL connection issues → Check XAMPP is running

## 🏃 Running the Applications

### Running the BackOffice (WPF Desktop Application)

1. **Set Startup Project:**
   - In Solution Explorer, **right-click** on `CarRental.BackOffice`
   - Select **Set as Startup Project**
   - The project name should appear in **bold**

2. **Run the Application:**
   - Press **F5** (Start Debugging) or **Ctrl + F5** (Start Without Debugging)
   - Or click the **▶️ Start** button in the toolbar
   - Or go to **Debug** → **Start Debugging**

3. **Login Window Appears:**
   - The login window will open
   - **Default Admin Credentials:**
     - Username: `admin`
     - Password: `Admin123!`

4. **First Run:**
   - On first run, the database will be created automatically
   - Sample data (vehicles, clients, etc.) will be seeded
   - This may take a few seconds

5. **Using the Application:**
   - After login, you'll see the main dashboard
   - Navigate using the menu on the left
   - Features: Dashboard, Clients, Vehicles, Rentals, Payments, etc.

### Running the Web Application (ASP.NET Core MVC)

1. **Set Startup Project:**
   - In Solution Explorer, **right-click** on `CarRental.Web`
   - Select **Set as Startup Project**
   - The project name should appear in **bold**

2. **Run the Application:**
   - Press **F5** (Start Debugging) or **Ctrl + F5** (Start Without Debugging)
   - Or click the **▶️ Start** button in the toolbar
   - Or go to **Debug** → **Start Debugging**

3. **Browser Opens Automatically:**
   - Visual Studio will launch your default browser
   - URL will be something like: `https://localhost:5001` or `http://localhost:5000`
   - If it doesn't open, check the Output window for the URL

4. **First Run:**
   - On first run, the database will be created automatically
   - Sample data will be seeded
   - This may take a few seconds

5. **Using the Web Application:**
   - **Browse Vehicles:** Click "Browse Vehicles" in the navigation
   - **Register:** Create a new client account
   - **Login:** Use test credentials (see Default Accounts below)
   - **Request Rental:** Select a vehicle and request a rental

### Running Both Applications Simultaneously

You can run both applications at the same time:

1. **Run BackOffice:**
   - Set `CarRental.BackOffice` as startup project
   - Press **Ctrl + F5** (Start Without Debugging) - this won't block Visual Studio

2. **Run Web Application:**
   - Set `CarRental.Web` as startup project
   - Press **F5** (Start Debugging)

**Note:** Both applications share the same database, so you can test the full workflow:
- Request a rental from the Web app
- Approve/deny it from the BackOffice app

## 🔐 Default Accounts

After the first run, the database is seeded with these accounts:

### Admin Account (for BackOffice)
- **Username:** `admin`
- **Password:** `Admin123!`
- **Role:** Admin

### Test Client Accounts (for Web Application)
1. **Username:** `john.doe`
   - **Password:** `Client123!`
   - **Email:** john.doe@email.com

2. **Username:** `jane.smith`
   - **Password:** `Client123!`
   - **Email:** jane.smith@email.com

## 📧 Email Configuration

Email functionality is optional but recommended. See `EMAIL_SETUP.md` for detailed instructions.

**Quick Setup (Gmail):**

1. **Get Gmail App Password:**
   - Enable 2-Step Verification on your Google account
   - Go to: https://myaccount.google.com/apppasswords
   - Generate an App Password for "Mail"

2. **Configure Web Application:**
   - Edit `CarRental.Web/appsettings.json`
   - Fill in your Gmail settings

3. **Configure BackOffice:**
   - Edit `CarRental.BackOffice/email.config`
   - Fill in your SMTP settings

**Note:** If email is not configured, the application will still work but emails won't be sent.

## 🛠️ Visual Studio Tips

### Solution Explorer
- **View:** View → Solution Explorer (or `Ctrl + Alt + L`)
- **Expand/Collapse:** Click the arrows next to folders
- **Search:** Use the search box at the top

### Debugging
- **Set Breakpoints:** Click in the left margin next to a line of code (red dot appears)
- **Step Through Code:** F10 (Step Over), F11 (Step Into), Shift + F11 (Step Out)
- **Watch Variables:** Right-click a variable → Add to Watch
- **View Output:** View → Output (or `Ctrl + Alt + O`)

### Error List
- **View Errors:** View → Error List (or `Ctrl + \`, `E`)
- **Navigate to Error:** Double-click an error to jump to the code

### Package Manager Console
- **Open:** Tools → NuGet Package Manager → Package Manager Console
- **Useful Commands:**
  - `Update-Package` - Update all packages
  - `Get-Package` - List installed packages

## 🐛 Troubleshooting

### "Cannot connect to database" Error

1. **Check XAMPP MySQL is Running:**
   - Open XAMPP Control Panel
   - Ensure MySQL shows "Running" (green)

2. **Verify Connection String:**
   - Check `CarRental.Data/DbConnectionHelper.cs`
   - Ensure port matches your XAMPP MySQL port (default: 3306 or 3307)

3. **Test MySQL Connection:**
   - Open phpMyAdmin: `http://localhost/phpmyadmin`
   - Try to login with root user (no password by default)

### "dotnet-ef not found" Error

1. **Install EF Core Tools:**
   ```bash
   dotnet tool install --global dotnet-ef --version 8.0.0
   ```

2. **Restart Visual Studio** after installation

3. **Verify Installation:**
   - Open Developer Command Prompt
   - Run: `dotnet ef --version`

### Build Errors

1. **Restore NuGet Packages:**
   - Right-click solution → Restore NuGet Packages

2. **Clean and Rebuild:**
   - Build → Clean Solution
   - Build → Rebuild Solution

3. **Check .NET Version:**
   - Tools → Get Tools and Features
   - Ensure .NET 8.0 SDK is installed

### Application Won't Start

1. **Check Startup Project:**
   - Right-click the project → Set as Startup Project

2. **Check for Errors:**
   - View → Error List
   - Fix any compilation errors

3. **Check Output Window:**
   - View → Output
   - Look for error messages

### Database Not Created

1. **Check XAMPP MySQL is Running**

2. **Check Connection String:**
   - Verify `DbConnectionHelper.cs` has correct settings

3. **Manually Create Database:**
   - Open phpMyAdmin
   - Create database named `CarRentalDb`
   - Run the application again

### Port Already in Use (Web Application)

If you see "Port 5000/5001 is already in use":

1. **Change Port in `launchSettings.json`:**
   - Navigate to: `CarRental.Web/Properties/launchSettings.json`
   - Change the port numbers to something else (e.g., 5002, 5003)

2. **Or Kill the Process:**
   - Open Task Manager
   - Find the process using the port
   - End the process

## 📝 Additional Notes

### Project Structure in Visual Studio

- **Solution Explorer** shows all projects and files
- **Solution** = `CarRentalStudent.sln` (contains all projects)
- **Projects:**
  - `CarRental.Domain` - Domain entities
  - `CarRental.Data` - Database layer
  - `CarRental.BackOffice` - WPF desktop app
  - `CarRental.Web` - MVC web app

### Switching Between Projects

To switch which project runs:

1. **Right-click** the project in Solution Explorer
2. Select **Set as Startup Project**
3. The project name will appear in **bold**

### Viewing Database

You can view and manage the database using:

- **phpMyAdmin:** `http://localhost/phpmyadmin`
- **Visual Studio Server Explorer:** View → Server Explorer → Data Connections

## 🎓 Quick Start Checklist

- [ ] Visual Studio 2022 installed
- [ ] .NET 8.0 SDK installed
- [ ] XAMPP installed and MySQL running
- [ ] Solution opened in Visual Studio
- [ ] NuGet packages restored
- [ ] EF Core tools installed
- [ ] Solution builds successfully
- [ ] Database connection configured
- [ ] BackOffice runs and login works
- [ ] Web application runs and opens in browser

## 📚 Related Documentation

- **Main README:** `README.md` - General project information
- **Email Setup:** `EMAIL_SETUP.md` - Detailed email configuration
- **Code Comments:** All code files have extensive comments explaining the logic

---

**Happy Coding with Visual Studio! 🚗**

If you encounter any issues not covered here, check the main `README.md` for additional troubleshooting tips.


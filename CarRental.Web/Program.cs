using Microsoft.EntityFrameworkCore;
using CarRental.Data;
using CarRental.Web.Services;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure database context
// This connects our application to the MySQL/MariaDB database (XAMPP)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(DbConnectionHelper.GetConnectionString(), 
        new MySqlServerVersion(new Version(8, 0, 21))));

// Configure session for storing user login information
// This allows us to track which user is logged in
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register services
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PdfService>();

var app = builder.Build();

// Initialize database on startup
// This ensures the database exists and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log the error in development
        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine($"Database initialization error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        // Database might already exist, that's okay
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

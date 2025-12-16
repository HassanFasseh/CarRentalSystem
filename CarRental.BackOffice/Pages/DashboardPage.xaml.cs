using System.Linq;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page displays the main dashboard with statistics
// It shows counts of clients, vehicles, active rentals, and total revenue
public partial class DashboardPage : Page
{
    private readonly DatabaseService _databaseService;

    // Constructor - receives the database service
    public DashboardPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        
        // Load and display statistics
        LoadStatistics();
    }

    // This method loads statistics from the database and displays them
    private void LoadStatistics()
    {
        using var context = _databaseService.GetContext();

        // Count total clients
        int totalClients = context.Clients.Count();
        TotalClientsTextBlock.Text = totalClients.ToString();

        // Count total vehicles
        int totalVehicles = context.Vehicles.Count();
        TotalVehiclesTextBlock.Text = totalVehicles.ToString();

        // Count active rentals (rentals that are currently active)
        int activeRentals = context.Rentals
            .Where(r => r.Status == RentalStatus.Active)
            .Count();
        ActiveRentalsTextBlock.Text = activeRentals.ToString();

        // Calculate total revenue from approved rentals
        // This includes all rentals that have been approved (Active or Completed)
        // This represents the total revenue from approved rental requests
        decimal totalRevenue = context.Rentals
            .Where(r => r.Status == RentalStatus.Active || 
                       r.Status == RentalStatus.Completed)
            .Sum(r => (decimal?)r.TotalAmount) ?? 0;
        
        TotalRevenueTextBlock.Text = totalRevenue.ToString("C");
    }
}


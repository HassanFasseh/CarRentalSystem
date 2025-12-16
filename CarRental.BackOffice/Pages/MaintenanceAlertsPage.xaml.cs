using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page shows vehicles that need maintenance based on last service date
public partial class MaintenanceAlertsPage : Page
{
    private readonly DatabaseService _databaseService;

    public MaintenanceAlertsPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        CheckButton_Click(null, null); // Auto-check on load
    }

    private void CheckButton_Click(object? sender, RoutedEventArgs? e)
    {
        if (!int.TryParse(DaysTextBox.Text, out int daysThreshold) || daysThreshold < 0)
        {
            MessageBox.Show("Please enter a valid number of days.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var context = _databaseService.GetContext();
        
        var vehicles = context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.LastServiceDate.HasValue)
            .ToList();

        var maintenanceNeeded = new List<MaintenanceAlert>();

        foreach (var vehicle in vehicles)
        {
            if (vehicle.LastServiceDate.HasValue)
            {
                int daysSinceService = (DateTime.Now - vehicle.LastServiceDate.Value).Days;
                
                if (daysSinceService >= daysThreshold)
                {
                    maintenanceNeeded.Add(new MaintenanceAlert
                    {
                        Id = vehicle.Id,
                        LicensePlate = vehicle.LicensePlate,
                        Make = vehicle.Make,
                        Model = vehicle.Model,
                        LastServiceDate = vehicle.LastServiceDate.Value,
                        DaysSinceService = daysSinceService
                    });
                }
            }
        }

        MaintenanceDataGrid.ItemsSource = maintenanceNeeded;
    }

    // Simple class to display maintenance alert information
    private class MaintenanceAlert
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime LastServiceDate { get; set; }
        public int DaysSinceService { get; set; }
    }
}


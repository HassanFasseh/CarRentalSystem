using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page checks vehicle availability for a date range
public partial class AvailabilityCheckPage : Page
{
    private readonly DatabaseService _databaseService;

    public AvailabilityCheckPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        
        // Set default dates
        StartDatePicker.SelectedDate = DateTime.Now;
        EndDatePicker.SelectedDate = DateTime.Now.AddDays(7);
    }

    private void CheckButton_Click(object sender, RoutedEventArgs e)
    {
        if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Please select both start and end dates.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (StartDatePicker.SelectedDate.Value > EndDatePicker.SelectedDate.Value)
        {
            MessageBox.Show("Start date must be before end date.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var context = _databaseService.GetContext();
        
        DateTime startDate = StartDatePicker.SelectedDate.Value;
        DateTime endDate = EndDatePicker.SelectedDate.Value;

        // Get all vehicles that are available
        var allVehicles = context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.Status == VehicleStatus.Available)
            .ToList();

        // Get vehicles that are rented during this period
        var rentedVehicleIds = context.Rentals
            .Where(r => r.Status == RentalStatus.Active &&
                       !(r.EndDate < startDate || r.StartDate > endDate))
            .Select(r => r.VehicleId)
            .ToList();

        // Filter out rented vehicles
        var availableVehicles = allVehicles
            .Where(v => !rentedVehicleIds.Contains(v.Id))
            .ToList();

        AvailableVehiclesDataGrid.ItemsSource = availableVehicles;
        
        MessageBox.Show($"Found {availableVehicles.Count} available vehicles.", "Availability Check", 
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}


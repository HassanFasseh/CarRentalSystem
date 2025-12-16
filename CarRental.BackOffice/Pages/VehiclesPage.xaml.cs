using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.BackOffice.Windows;
using CarRental.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page manages vehicles (CRUD operations)
public partial class VehiclesPage : Page
{
    private readonly DatabaseService _databaseService;
    private Vehicle? _selectedVehicle;

    public VehiclesPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadVehicles();
    }

    private void LoadVehicles()
    {
        using var context = _databaseService.GetContext();
        var vehicles = context.Vehicles
            .Include(v => v.VehicleType)
            .ToList();
        
        VehiclesDataGrid.ItemsSource = vehicles;
        StatusTextBlock.Text = $"Total vehicles: {vehicles.Count}";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new VehicleEditWindow(_databaseService, null);
        if (window.ShowDialog() == true)
        {
            LoadVehicles();
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedVehicle == null)
        {
            MessageBox.Show("Please select a vehicle to edit.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var window = new VehicleEditWindow(_databaseService, _selectedVehicle);
        if (window.ShowDialog() == true)
        {
            LoadVehicles();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedVehicle == null)
        {
            MessageBox.Show("Please select a vehicle to delete.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete vehicle {_selectedVehicle.LicensePlate}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            bool hasRentals = context.Rentals.Any(r => r.VehicleId == _selectedVehicle.Id);
            
            if (hasRentals)
            {
                MessageBox.Show("Cannot delete vehicle with existing rentals.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Vehicles.Remove(_selectedVehicle);
            context.SaveChanges();
            LoadVehicles();
            MessageBox.Show("Vehicle deleted successfully.", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadVehicles();
    }

    private void VehiclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedVehicle = VehiclesDataGrid.SelectedItem as Vehicle;
    }
}


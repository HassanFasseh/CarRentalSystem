using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.BackOffice.Windows;
using CarRental.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page manages vehicle types (CRUD operations)
public partial class VehicleTypesPage : Page
{
    private readonly DatabaseService _databaseService;
    private VehicleType? _selectedVehicleType;

    public VehicleTypesPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadVehicleTypes();
    }

    private void LoadVehicleTypes()
    {
        using var context = _databaseService.GetContext();
        var vehicleTypes = context.VehicleTypes.ToList();
        VehicleTypesDataGrid.ItemsSource = vehicleTypes;
        StatusTextBlock.Text = $"Total vehicle types: {vehicleTypes.Count}";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new VehicleTypeEditWindow(_databaseService, null);
        if (window.ShowDialog() == true)
        {
            LoadVehicleTypes();
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedVehicleType == null)
        {
            MessageBox.Show("Please select a vehicle type to edit.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var window = new VehicleTypeEditWindow(_databaseService, _selectedVehicleType);
        if (window.ShowDialog() == true)
        {
            LoadVehicleTypes();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedVehicleType == null)
        {
            MessageBox.Show("Please select a vehicle type to delete.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete vehicle type {_selectedVehicleType.Name}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            bool hasVehicles = context.Vehicles.Any(v => v.VehicleTypeId == _selectedVehicleType.Id);
            
            if (hasVehicles)
            {
                MessageBox.Show("Cannot delete vehicle type with existing vehicles.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.VehicleTypes.Remove(_selectedVehicleType);
            context.SaveChanges();
            LoadVehicleTypes();
            MessageBox.Show("Vehicle type deleted successfully.", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadVehicleTypes();
    }

    private void VehicleTypesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedVehicleType = VehicleTypesDataGrid.SelectedItem as VehicleType;
    }
}


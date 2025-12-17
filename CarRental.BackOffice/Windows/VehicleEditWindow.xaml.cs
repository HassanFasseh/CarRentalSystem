using System;
using System.Linq;
using System.Windows;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;

namespace CarRental.BackOffice.Windows;

// This window is used to add or edit a vehicle
public partial class VehicleEditWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly Vehicle? _vehicle;

    public VehicleEditWindow(DatabaseService databaseService, Vehicle? vehicle)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _vehicle = vehicle;

        LoadVehicleTypes();
        LoadStatuses();

        if (_vehicle != null)
        {
            Title = "Edit Vehicle";
            LicensePlateTextBox.Text = _vehicle.LicensePlate;
            MakeTextBox.Text = _vehicle.Make;
            ModelTextBox.Text = _vehicle.Model;
            YearTextBox.Text = _vehicle.Year.ToString();
            MileageTextBox.Text = _vehicle.Mileage.ToString();
            
            var vehicleType = VehicleTypeComboBox.Items.Cast<VehicleType>()
                .FirstOrDefault(vt => vt.Id == _vehicle.VehicleTypeId);
            if (vehicleType != null)
            {
                VehicleTypeComboBox.SelectedItem = vehicleType;
            }
            
            // Find matching status in the combobox items
            var statuses = StatusComboBox.ItemsSource.Cast<VehicleStatus>();
            var matchingStatus = statuses.FirstOrDefault(s => s == _vehicle.Status);
            if (matchingStatus != null)
            {
                StatusComboBox.SelectedItem = matchingStatus;
            }
            
        }
        else
        {
            Title = "Add New Vehicle";
            StatusComboBox.SelectedItem = VehicleStatus.Available;
        }
    }

    private void LoadVehicleTypes()
    {
        using var context = _databaseService.GetContext();
        var vehicleTypes = context.VehicleTypes.ToList();
        VehicleTypeComboBox.ItemsSource = vehicleTypes;
        VehicleTypeComboBox.DisplayMemberPath = "Name";
    }

    private void LoadStatuses()
    {
        var statuses = Enum.GetValues(typeof(VehicleStatus)).Cast<VehicleStatus>().ToList();
        StatusComboBox.ItemsSource = statuses;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleTypeComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select a vehicle type.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (StatusComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select a status.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(LicensePlateTextBox.Text))
        {
            MessageBox.Show("Please enter license plate.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(YearTextBox.Text, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
        {
            MessageBox.Show("Please enter a valid year.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!int.TryParse(MileageTextBox.Text, out int mileage) || mileage < 0)
        {
            MessageBox.Show("Please enter a valid mileage.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var context = _databaseService.GetContext();
        
        if (_vehicle == null)
        {
            var newVehicle = new Vehicle
            {
                VehicleTypeId = ((VehicleType)VehicleTypeComboBox.SelectedItem).Id,
                LicensePlate = LicensePlateTextBox.Text.Trim(),
                Make = MakeTextBox.Text.Trim(),
                Model = ModelTextBox.Text.Trim(),
                Year = year,
                Color = string.Empty, // Color will be set separately if needed
                Status = (VehicleStatus)StatusComboBox.SelectedItem,
                Mileage = mileage,
                LastServiceDate = null,
                CreatedAt = DateTime.Now
            };
            
            context.Vehicles.Add(newVehicle);
        }
        else
        {
            // Reload vehicle from current context to avoid tracking issues
            var vehicleToUpdate = context.Vehicles.Find(_vehicle.Id);
            if (vehicleToUpdate == null)
            {
                MessageBox.Show("Vehicle not found in database.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            vehicleToUpdate.VehicleTypeId = ((VehicleType)VehicleTypeComboBox.SelectedItem).Id;
            vehicleToUpdate.LicensePlate = LicensePlateTextBox.Text.Trim();
            vehicleToUpdate.Make = MakeTextBox.Text.Trim();
            vehicleToUpdate.Model = ModelTextBox.Text.Trim();
            vehicleToUpdate.Year = year;
            // Color remains unchanged
            vehicleToUpdate.Status = (VehicleStatus)StatusComboBox.SelectedItem;
            vehicleToUpdate.Mileage = mileage;
            // LastServiceDate remains unchanged
        }

        try
        {
            context.SaveChanges();
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving vehicle: {ex.Message}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}


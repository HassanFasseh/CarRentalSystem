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
            ColorTextBox.Text = _vehicle.Color;
            MileageTextBox.Text = _vehicle.Mileage.ToString();
            
            var vehicleType = VehicleTypeComboBox.Items.Cast<VehicleType>()
                .FirstOrDefault(vt => vt.Id == _vehicle.VehicleTypeId);
            if (vehicleType != null)
            {
                VehicleTypeComboBox.SelectedItem = vehicleType;
            }
            
            StatusComboBox.SelectedItem = _vehicle.Status;
            
            if (_vehicle.LastServiceDate.HasValue)
            {
                LastServiceDatePicker.SelectedDate = _vehicle.LastServiceDate.Value;
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
        StatusComboBox.ItemsSource = Enum.GetValues(typeof(VehicleStatus));
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleTypeComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select a vehicle type.", "Validation Error", 
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
                Color = ColorTextBox.Text.Trim(),
                Status = (VehicleStatus)StatusComboBox.SelectedItem,
                Mileage = mileage,
                LastServiceDate = LastServiceDatePicker.SelectedDate,
                CreatedAt = DateTime.Now
            };
            
            context.Vehicles.Add(newVehicle);
        }
        else
        {
            _vehicle.VehicleTypeId = ((VehicleType)VehicleTypeComboBox.SelectedItem).Id;
            _vehicle.LicensePlate = LicensePlateTextBox.Text.Trim();
            _vehicle.Make = MakeTextBox.Text.Trim();
            _vehicle.Model = ModelTextBox.Text.Trim();
            _vehicle.Year = year;
            _vehicle.Color = ColorTextBox.Text.Trim();
            _vehicle.Status = (VehicleStatus)StatusComboBox.SelectedItem;
            _vehicle.Mileage = mileage;
            _vehicle.LastServiceDate = LastServiceDatePicker.SelectedDate;
            
            context.Vehicles.Update(_vehicle);
        }

        context.SaveChanges();
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}


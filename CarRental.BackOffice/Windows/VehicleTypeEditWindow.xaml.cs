using System.Windows;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Entities;

namespace CarRental.BackOffice.Windows;

// This window is used to add or edit a vehicle type
public partial class VehicleTypeEditWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly VehicleType? _vehicleType;

    public VehicleTypeEditWindow(DatabaseService databaseService, VehicleType? vehicleType)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _vehicleType = vehicleType;

        if (_vehicleType != null)
        {
            Title = "Edit Vehicle Type";
            NameTextBox.Text = _vehicleType.Name;
            DescriptionTextBox.Text = _vehicleType.Description;
            DailyRateTextBox.Text = _vehicleType.DailyRate.ToString("F2");
        }
        else
        {
            Title = "Add New Vehicle Type";
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameTextBox.Text))
        {
            MessageBox.Show("Please enter a name.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(DailyRateTextBox.Text, out decimal dailyRate) || dailyRate < 0)
        {
            MessageBox.Show("Please enter a valid daily rate.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var context = _databaseService.GetContext();
        
        if (_vehicleType == null)
        {
            var newVehicleType = new VehicleType
            {
                Name = NameTextBox.Text.Trim(),
                Description = DescriptionTextBox.Text.Trim(),
                DailyRate = dailyRate
            };
            
            context.VehicleTypes.Add(newVehicleType);
        }
        else
        {
            _vehicleType.Name = NameTextBox.Text.Trim();
            _vehicleType.Description = DescriptionTextBox.Text.Trim();
            _vehicleType.DailyRate = dailyRate;
            
            context.VehicleTypes.Update(_vehicleType);
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


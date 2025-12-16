using System;
using System.Linq;
using System.Windows;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Windows;

// This window is used to add or edit a rental
public partial class RentalEditWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly Rental? _rental;

    public RentalEditWindow(DatabaseService databaseService, Rental? rental)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _rental = rental;

        LoadClients();
        LoadVehicles();
        LoadStatuses();

        if (_rental != null)
        {
            Title = "Edit Rental";
            StartDatePicker.SelectedDate = _rental.StartDate;
            EndDatePicker.SelectedDate = _rental.EndDate;
            DailyRateTextBox.Text = _rental.DailyRate.ToString("F2");
            
            var client = ClientComboBox.Items.Cast<Client>()
                .FirstOrDefault(c => c.Id == _rental.ClientId);
            if (client != null) ClientComboBox.SelectedItem = client;
            
            var vehicle = VehicleComboBox.Items.Cast<Vehicle>()
                .FirstOrDefault(v => v.Id == _rental.VehicleId);
            if (vehicle != null) VehicleComboBox.SelectedItem = vehicle;
            
            StatusComboBox.SelectedItem = _rental.Status;
        }
        else
        {
            Title = "Add New Rental";
            StartDatePicker.SelectedDate = DateTime.Now;
            EndDatePicker.SelectedDate = DateTime.Now.AddDays(7);
        }
    }

    private void LoadClients()
    {
        using var context = _databaseService.GetContext();
        var clients = context.Clients.ToList();
        ClientComboBox.ItemsSource = clients;
        ClientComboBox.DisplayMemberPath = "FirstName";
    }

    private void LoadVehicles()
    {
        using var context = _databaseService.GetContext();
        var vehicles = context.Vehicles
            .Include(v => v.VehicleType)
            .Where(v => v.Status == VehicleStatus.Available)
            .ToList();
        VehicleComboBox.ItemsSource = vehicles;
        VehicleComboBox.DisplayMemberPath = "LicensePlate";
    }

    private void LoadStatuses()
    {
        StatusComboBox.ItemsSource = Enum.GetValues(typeof(RentalStatus));
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (ClientComboBox.SelectedItem == null || VehicleComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select both client and vehicle.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

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

        if (!decimal.TryParse(DailyRateTextBox.Text, out decimal dailyRate) || dailyRate < 0)
        {
            MessageBox.Show("Please enter a valid daily rate.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var context = _databaseService.GetContext();
        
        DateTime startDate = StartDatePicker.SelectedDate.Value;
        DateTime endDate = EndDatePicker.SelectedDate.Value;
        int numberOfDays = (endDate - startDate).Days + 1;
        decimal totalAmount = dailyRate * numberOfDays;

        // Get first employee for new rentals
        var employee = context.Employees.FirstOrDefault();
        if (employee == null)
        {
            MessageBox.Show("No employees found. Please create an employee first.", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (_rental == null)
        {
            var newRental = new Rental
            {
                ClientId = ((Client)ClientComboBox.SelectedItem).Id,
                VehicleId = ((Vehicle)VehicleComboBox.SelectedItem).Id,
                EmployeeId = employee.Id,
                StartDate = startDate,
                EndDate = endDate,
                NumberOfDays = numberOfDays,
                DailyRate = dailyRate,
                TotalAmount = totalAmount,
                Status = (RentalStatus)StatusComboBox.SelectedItem,
                CreatedAt = DateTime.Now
            };
            
            context.Rentals.Add(newRental);
        }
        else
        {
            _rental.ClientId = ((Client)ClientComboBox.SelectedItem).Id;
            _rental.VehicleId = ((Vehicle)VehicleComboBox.SelectedItem).Id;
            _rental.StartDate = startDate;
            _rental.EndDate = endDate;
            _rental.NumberOfDays = numberOfDays;
            _rental.DailyRate = dailyRate;
            _rental.TotalAmount = totalAmount;
            _rental.Status = (RentalStatus)StatusComboBox.SelectedItem;
            
            context.Rentals.Update(_rental);
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


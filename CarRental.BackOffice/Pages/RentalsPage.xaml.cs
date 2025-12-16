using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.BackOffice.Windows;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page manages rentals (CRUD operations)
public partial class RentalsPage : Page
{
    private readonly DatabaseService _databaseService;
    private Rental? _selectedRental;

    public RentalsPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadRentals();
    }

    private void LoadRentals()
    {
        using var context = _databaseService.GetContext();
        var rentals = context.Rentals
            .Include(r => r.Client)
            .Include(r => r.Vehicle)
            .Include(r => r.Employee)
            .ToList();
        
        RentalsDataGrid.ItemsSource = rentals;
        StatusTextBlock.Text = $"Total rentals: {rentals.Count}";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new RentalEditWindow(_databaseService, null);
        if (window.ShowDialog() == true)
        {
            LoadRentals();
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRental == null)
        {
            MessageBox.Show("Please select a rental to edit.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var window = new RentalEditWindow(_databaseService, _selectedRental);
        if (window.ShowDialog() == true)
        {
            LoadRentals();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRental == null)
        {
            MessageBox.Show("Please select a rental to delete.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete rental #{_selectedRental.Id}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            bool hasPayment = context.Payments.Any(p => p.RentalId == _selectedRental.Id);
            
            if (hasPayment)
            {
                MessageBox.Show("Cannot delete rental with existing payment.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            context.Rentals.Remove(_selectedRental);
            context.SaveChanges();
            LoadRentals();
            MessageBox.Show("Rental deleted successfully.", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void GeneratePdfButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRental == null)
        {
            MessageBox.Show("Please select a rental to generate PDF.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var pdfService = new Services.PdfService(_databaseService);
        pdfService.GenerateReservationPdf(_selectedRental);
    }

    private void GenerateQrCodeButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRental == null)
        {
            MessageBox.Show("Please select a rental to generate QR code.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        string qrText = $"Rental ID: {_selectedRental.Id}\nClient: {_selectedRental.Client?.FirstName} {_selectedRental.Client?.LastName}\nVehicle: {_selectedRental.Vehicle?.LicensePlate}\nDates: {_selectedRental.StartDate:yyyy-MM-dd} to {_selectedRental.EndDate:yyyy-MM-dd}";
        
        var qrWindow = new Windows.QrCodeWindow(qrText);
        qrWindow.ShowDialog();
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadRentals();
    }

    private void ApproveButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRental == null)
        {
            MessageBox.Show("Please select a rental to approve.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (_selectedRental.Status != RentalStatus.Pending)
        {
            MessageBox.Show("Only pending rentals can be approved.", "Invalid Status", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to approve rental #{_selectedRental.Id}?",
            "Confirm Approval",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            
            // Reload the rental with all related data
            var rental = context.Rentals
                .Include(r => r.Client)
                .ThenInclude(c => c!.User)
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == _selectedRental.Id);
            
            if (rental != null)
            {
                rental.Status = RentalStatus.Active;
                
                // Update vehicle status to Rented
                var vehicle = context.Vehicles.FirstOrDefault(v => v.Id == rental.VehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = VehicleStatus.Rented;
                }
                
                context.SaveChanges();
                LoadRentals();
                
                // Send approval email to client (async, don't wait)
                if (rental.Client?.User != null && !string.IsNullOrEmpty(rental.Client.User.Email))
                {
                    var emailService = new Services.EmailService();
                    _ = Task.Run(async () =>
                    {
                        await emailService.SendRentalApprovalAsync(
                            rental.Client.User.Email,
                            $"{rental.Client.FirstName} {rental.Client.LastName}",
                            rental.Id,
                            $"{rental.Vehicle?.Make} {rental.Vehicle?.Model} ({rental.Vehicle?.LicensePlate})",
                            rental.StartDate,
                            rental.EndDate,
                            rental.TotalAmount
                        );
                    });
                }
                
                MessageBox.Show("Rental approved successfully.", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private void DenyButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedRental == null)
        {
            MessageBox.Show("Please select a rental to deny.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (_selectedRental.Status != RentalStatus.Pending)
        {
            MessageBox.Show("Only pending rentals can be denied.", "Invalid Status", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to deny rental #{_selectedRental.Id}?",
            "Confirm Denial",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            
            // Reload the rental with all related data
            var rental = context.Rentals
                .Include(r => r.Client)
                .ThenInclude(c => c!.User)
                .Include(r => r.Vehicle)
                .FirstOrDefault(r => r.Id == _selectedRental.Id);
            
            if (rental != null)
            {
                rental.Status = RentalStatus.Denied;
                context.SaveChanges();
                LoadRentals();
                
                // Send denial email to client (async, don't wait)
                if (rental.Client?.User != null && !string.IsNullOrEmpty(rental.Client.User.Email))
                {
                    var emailService = new Services.EmailService();
                    _ = Task.Run(async () =>
                    {
                        await emailService.SendRentalDenialAsync(
                            rental.Client.User.Email,
                            $"{rental.Client.FirstName} {rental.Client.LastName}",
                            rental.Id,
                            $"{rental.Vehicle?.Make} {rental.Vehicle?.Model} ({rental.Vehicle?.LicensePlate})",
                            rental.StartDate,
                            rental.EndDate
                        );
                    });
                }
                
                MessageBox.Show("Rental denied successfully.", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private void RentalsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedRental = RentalsDataGrid.SelectedItem as Rental;
    }
}


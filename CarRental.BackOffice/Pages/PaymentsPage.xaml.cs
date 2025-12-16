using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.BackOffice.Windows;
using CarRental.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page manages payments (CRUD operations)
public partial class PaymentsPage : Page
{
    private readonly DatabaseService _databaseService;
    private Payment? _selectedPayment;

    public PaymentsPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadPayments();
    }

    private void LoadPayments()
    {
        using var context = _databaseService.GetContext();
        var payments = context.Payments
            .Include(p => p.Rental)
            .ToList();
        
        PaymentsDataGrid.ItemsSource = payments;
        StatusTextBlock.Text = $"Total payments: {payments.Count}";
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new PaymentEditWindow(_databaseService, null);
        if (window.ShowDialog() == true)
        {
            LoadPayments();
        }
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedPayment == null)
        {
            MessageBox.Show("Please select a payment to edit.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var window = new PaymentEditWindow(_databaseService, _selectedPayment);
        if (window.ShowDialog() == true)
        {
            LoadPayments();
        }
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedPayment == null)
        {
            MessageBox.Show("Please select a payment to delete.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete payment #{_selectedPayment.Id}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            context.Payments.Remove(_selectedPayment);
            context.SaveChanges();
            LoadPayments();
            MessageBox.Show("Payment deleted successfully.", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadPayments();
    }

    private void PaymentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedPayment = PaymentsDataGrid.SelectedItem as Payment;
    }
}


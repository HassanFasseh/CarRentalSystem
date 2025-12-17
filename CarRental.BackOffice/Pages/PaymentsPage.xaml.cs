using System;
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
        Loaded += PaymentsPage_Loaded;
    }

    private void PaymentsPage_Loaded(object sender, RoutedEventArgs e)
    {
        LoadPayments();
    }

    private void LoadPayments()
    {
        try
        {
            using var context = _databaseService.GetContext();
            var payments = context.Payments
                .Include(p => p.Rental)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();
            
            PaymentsDataGrid.ItemsSource = null;
            PaymentsDataGrid.ItemsSource = payments;
            StatusTextBlock.Text = $"Total payments: {payments.Count}";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading payments: {ex.Message}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            StatusTextBlock.Text = "Error loading payments";
            PaymentsDataGrid.ItemsSource = null;
        }
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
            try
            {
                using var context = _databaseService.GetContext();
                var paymentToDelete = context.Payments.Find(_selectedPayment.Id);
                if (paymentToDelete != null)
                {
                    context.Payments.Remove(paymentToDelete);
                    context.SaveChanges();
                    LoadPayments();
                    MessageBox.Show("Payment deleted successfully.", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting payment: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
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


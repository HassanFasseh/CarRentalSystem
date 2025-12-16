using System;
using System.Linq;
using System.Windows;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Windows;

// This window is used to add or edit a payment
public partial class PaymentEditWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly Payment? _payment;

    public PaymentEditWindow(DatabaseService databaseService, Payment? payment)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _payment = payment;

        LoadRentals();
        LoadStatuses();

        if (_payment != null)
        {
            Title = "Edit Payment";
            AmountTextBox.Text = _payment.Amount.ToString("F2");
            PaymentMethodTextBox.Text = _payment.PaymentMethod;
            PaymentDatePicker.SelectedDate = _payment.PaymentDate;
            
            var rental = RentalComboBox.Items.Cast<Rental>()
                .FirstOrDefault(r => r.Id == _payment.RentalId);
            if (rental != null) RentalComboBox.SelectedItem = rental;
            
            StatusComboBox.SelectedItem = _payment.Status;
        }
        else
        {
            Title = "Add New Payment";
            PaymentDatePicker.SelectedDate = DateTime.Now;
        }
    }

    private void LoadRentals()
    {
        using var context = _databaseService.GetContext();
        var rentals = context.Rentals.ToList();
        RentalComboBox.ItemsSource = rentals;
        RentalComboBox.DisplayMemberPath = "Id";
    }

    private void LoadStatuses()
    {
        StatusComboBox.ItemsSource = Enum.GetValues(typeof(PaymentStatus));
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        if (RentalComboBox.SelectedItem == null)
        {
            MessageBox.Show("Please select a rental.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount < 0)
        {
            MessageBox.Show("Please enter a valid amount.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(PaymentMethodTextBox.Text))
        {
            MessageBox.Show("Please enter a payment method.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!PaymentDatePicker.SelectedDate.HasValue)
        {
            MessageBox.Show("Please select a payment date.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        using var context = _databaseService.GetContext();
        
        if (_payment == null)
        {
            var newPayment = new Payment
            {
                RentalId = ((Rental)RentalComboBox.SelectedItem).Id,
                Amount = amount,
                Status = (PaymentStatus)StatusComboBox.SelectedItem,
                PaymentMethod = PaymentMethodTextBox.Text.Trim(),
                PaymentDate = PaymentDatePicker.SelectedDate.Value,
                CreatedAt = DateTime.Now
            };
            
            context.Payments.Add(newPayment);
        }
        else
        {
            _payment.RentalId = ((Rental)RentalComboBox.SelectedItem).Id;
            _payment.Amount = amount;
            _payment.Status = (PaymentStatus)StatusComboBox.SelectedItem;
            _payment.PaymentMethod = PaymentMethodTextBox.Text.Trim();
            _payment.PaymentDate = PaymentDatePicker.SelectedDate.Value;
            
            context.Payments.Update(_payment);
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


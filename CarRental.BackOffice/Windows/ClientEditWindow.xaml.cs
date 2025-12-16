using System.Linq;
using System.Windows;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Windows;

// This window is used to add or edit a client
// It allows entering client information and linking to a user account
public partial class ClientEditWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly Client? _client;

    // Constructor - receives database service and optional client to edit
    public ClientEditWindow(DatabaseService databaseService, Client? client)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _client = client;

        // Load available users (only client role users without a client record)
        LoadUsers();

        // If editing, populate the form with existing data
        if (_client != null)
        {
            Title = "Edit Client";
            UserComboBox.IsEnabled = false; // Can't change user when editing
            FirstNameTextBox.Text = _client.FirstName;
            LastNameTextBox.Text = _client.LastName;
            PhoneNumberTextBox.Text = _client.PhoneNumber;
            AddressTextBox.Text = _client.Address;
            DriverLicenseTextBox.Text = _client.DriverLicenseNumber;
            
            // Select the current user
            var currentUser = UserComboBox.Items.Cast<Domain.Entities.User>()
                .FirstOrDefault(u => u.Id == _client.UserId);
            if (currentUser != null)
            {
                UserComboBox.SelectedItem = currentUser;
            }
        }
        else
        {
            Title = "Add New Client";
        }
    }

    // This method loads available users for client creation
    private void LoadUsers()
    {
        using var context = _databaseService.GetContext();
        
        // Get all client role users
        var clientUsers = context.Users
            .Where(u => u.Role == UserRole.Client && u.IsActive)
            .ToList();

        // If editing, include the current user
        if (_client != null)
        {
            var currentUser = context.Users.FirstOrDefault(u => u.Id == _client.UserId);
            if (currentUser != null && !clientUsers.Contains(currentUser))
            {
                clientUsers.Add(currentUser);
            }
        }
        else
        {
            // For new clients, exclude users that already have a client record
            var usersWithClients = context.Clients.Select(c => c.UserId).ToList();
            clientUsers = clientUsers.Where(u => !usersWithClients.Contains(u.Id)).ToList();
        }

        UserComboBox.ItemsSource = clientUsers;
        UserComboBox.DisplayMemberPath = "Username";
    }

    // This method is called when the save button is clicked
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Validate input
        if (UserComboBox.SelectedItem == null && _client == null)
        {
            MessageBox.Show("Please select a user.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
        {
            MessageBox.Show("Please enter first name.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
        {
            MessageBox.Show("Please enter last name.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(DriverLicenseTextBox.Text))
        {
            MessageBox.Show("Please enter driver license number.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Save the client
        using var context = _databaseService.GetContext();
        
        if (_client == null)
        {
            // Create new client
            var selectedUser = UserComboBox.SelectedItem as Domain.Entities.User;
            if (selectedUser == null)
            {
                MessageBox.Show("Please select a user.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            var newClient = new Client
            {
                UserId = selectedUser.Id,
                FirstName = FirstNameTextBox.Text.Trim(),
                LastName = LastNameTextBox.Text.Trim(),
                PhoneNumber = PhoneNumberTextBox.Text.Trim(),
                Address = AddressTextBox.Text.Trim(),
                DriverLicenseNumber = DriverLicenseTextBox.Text.Trim(),
                CreatedAt = DateTime.Now
            };
            
            context.Clients.Add(newClient);
        }
        else
        {
            // Update existing client
            _client.FirstName = FirstNameTextBox.Text.Trim();
            _client.LastName = LastNameTextBox.Text.Trim();
            _client.PhoneNumber = PhoneNumberTextBox.Text.Trim();
            _client.Address = AddressTextBox.Text.Trim();
            _client.DriverLicenseNumber = DriverLicenseTextBox.Text.Trim();
            
            context.Clients.Update(_client);
        }

        context.SaveChanges();
        DialogResult = true;
        Close();
    }

    // This method is called when the cancel button is clicked
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}


using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;
using CarRental.BackOffice.Windows;
using CarRental.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Pages;

// This page manages clients (CRUD operations)
// Users can view, add, edit, and delete clients
public partial class ClientsPage : Page
{
    private readonly DatabaseService _databaseService;
    private Client? _selectedClient;

    // Constructor - receives the database service
    public ClientsPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
        LoadClients();
    }

    // This method loads all clients from the database and displays them
    private void LoadClients()
    {
        using var context = _databaseService.GetContext();
        
        // Load clients with their related user information
        var clients = context.Clients
            .Include(c => c.User)
            .ToList();
        
        ClientsDataGrid.ItemsSource = clients;
        StatusTextBlock.Text = $"Total clients: {clients.Count}";
    }

    // This method is called when the add button is clicked
    // It opens a window to add a new client
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var window = new ClientEditWindow(_databaseService, null);
        if (window.ShowDialog() == true)
        {
            LoadClients(); // Refresh the list
        }
    }

    // This method is called when the edit button is clicked
    // It opens a window to edit the selected client
    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedClient == null)
        {
            MessageBox.Show("Please select a client to edit.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var window = new ClientEditWindow(_databaseService, _selectedClient);
        if (window.ShowDialog() == true)
        {
            LoadClients(); // Refresh the list
        }
    }

    // This method is called when the delete button is clicked
    // It deletes the selected client after confirmation
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedClient == null)
        {
            MessageBox.Show("Please select a client to delete.", "No Selection", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Ask for confirmation
        var result = MessageBox.Show(
            $"Are you sure you want to delete client {_selectedClient.FirstName} {_selectedClient.LastName}?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = _databaseService.GetContext();
            
            // Check if client has any rentals
            bool hasRentals = context.Rentals.Any(r => r.ClientId == _selectedClient.Id);
            
            if (hasRentals)
            {
                MessageBox.Show("Cannot delete client with existing rentals.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Delete the client
            context.Clients.Remove(_selectedClient);
            context.SaveChanges();
            
            LoadClients(); // Refresh the list
            MessageBox.Show("Client deleted successfully.", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // This method is called when the refresh button is clicked
    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadClients();
    }

    // This method is called when the selection in the data grid changes
    private void ClientsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedClient = ClientsDataGrid.SelectedItem as Client;
    }
}


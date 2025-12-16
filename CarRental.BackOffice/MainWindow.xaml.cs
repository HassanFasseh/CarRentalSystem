using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Pages;
using CarRental.BackOffice.Services;
using CarRental.BackOffice.Windows;
using CarRental.Domain.Entities;

namespace CarRental.BackOffice;

// This is the main window of the back office application
// It provides navigation to all the different sections
public partial class MainWindow : Window
{
    private readonly DatabaseService _databaseService;
    private readonly User _currentUser;

    // Constructor - receives the database service and current user
    public MainWindow(DatabaseService databaseService, User currentUser)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _currentUser = currentUser;
        
        // Display user information
        UserInfoTextBlock.Text = $"Logged in as:\n{_currentUser.Username}\n({_currentUser.Email})";
        
        // Show dashboard by default
        ShowDashboard();
    }

    // This method shows the dashboard page
    private void DashboardButton_Click(object sender, RoutedEventArgs e)
    {
        ShowDashboard();
    }

    // This method shows the clients management page
    private void ClientsButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new ClientsPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the vehicles management page
    private void VehiclesButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new VehiclesPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the vehicle types management page
    private void VehicleTypesButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new VehicleTypesPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the rentals management page
    private void RentalsButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new RentalsPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the payments management page
    private void PaymentsButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new PaymentsPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the availability check page
    private void AvailabilityButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new AvailabilityCheckPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the maintenance alerts page
    private void MaintenanceButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new MaintenanceAlertsPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the CSV export page
    private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new ExportCsvPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method shows the CSV import page
    private void ImportCsvButton_Click(object sender, RoutedEventArgs e)
    {
        var page = new ImportCsvPage(_databaseService);
        ContentFrame.Navigate(page);
    }

    // This method displays the dashboard page
    private void ShowDashboard()
    {
        var page = new DashboardPage(_databaseService);
        ContentFrame.Navigate(page);
    }
}

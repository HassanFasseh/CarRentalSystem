using System.Security.Cryptography;
using System.Text;
using System.Windows;
using CarRental.BackOffice.Services;
using CarRental.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CarRental.BackOffice.Windows;

// This window handles user login
// Users must enter valid admin credentials to access the back office
public partial class LoginWindow : Window
{
    private readonly DatabaseService _databaseService;

    // Constructor - initializes the database service
    public LoginWindow()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        
        // Initialize the database when the app starts
        _databaseService.InitializeDatabase();
        
        // Set focus to username field
        UsernameTextBox.Focus();
    }

    // This method is called when the login button is clicked
    // It validates the username and password
    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        // Get username and password from the form
        string username = UsernameTextBox.Text.Trim();
        string password = PasswordBox.Password;

        // Check if fields are empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Please enter both username and password.");
            return;
        }

        // Try to find the user in the database
        using var context = _databaseService.GetContext();
        var user = context.Users
            .FirstOrDefault(u => u.Username == username && u.IsActive);

        // Check if user exists
        if (user == null)
        {
            ShowError("Invalid username or password.");
            return;
        }

        // Check if user is an admin
        if (user.Role != UserRole.Admin)
        {
            ShowError("Access denied. Admin privileges required.");
            return;
        }

        // Hash the entered password and compare with stored hash
        string hashedPassword = HashPassword(password);
        if (user.PasswordHash != hashedPassword)
        {
            ShowError("Invalid username or password.");
            return;
        }

        // Login successful - open main window
        var mainWindow = new MainWindow(_databaseService, user);
        mainWindow.Show();
        this.Close();
    }

    // This method displays an error message to the user
    private void ShowError(string message)
    {
        ErrorMessageTextBlock.Text = message;
        ErrorMessageTextBlock.Visibility = Visibility.Visible;
    }

    // This method hashes a password using SHA256
    // It uses the same method as the seed data
    private static string HashPassword(string password)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] hashBytes = SHA256.HashData(passwordBytes);
        
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            stringBuilder.Append(hashBytes[i].ToString("x2"));
        }
        
        return stringBuilder.ToString();
    }
}


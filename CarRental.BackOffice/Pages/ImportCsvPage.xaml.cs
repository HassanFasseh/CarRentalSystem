using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using CarRental.BackOffice.Services;

namespace CarRental.BackOffice.Pages;

// This page allows importing data from CSV files
public partial class ImportCsvPage : Page
{
    private readonly DatabaseService _databaseService;

    public ImportCsvPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            Title = "Select CSV file to import"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            FilePathTextBox.Text = openFileDialog.FileName;
        }
    }

    private void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FilePathTextBox.Text))
        {
            MessageBox.Show("Please select a CSV file.", "Validation Error", 
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var selectedItem = DataTypeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
        if (selectedItem == null)
        {
            return;
        }

        string dataType = selectedItem.Content.ToString() ?? "";
        var csvService = new Services.CsvService(_databaseService);
        
        try
        {
            int importedCount = csvService.ImportFromCsv(FilePathTextBox.Text, dataType);
            StatusTextBlock.Text = $"Successfully imported {importedCount} records.";
            MessageBox.Show($"Successfully imported {importedCount} records.", "Import Complete", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Error importing data: {ex.Message}", "Import Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


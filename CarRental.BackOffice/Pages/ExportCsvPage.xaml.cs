using System.Windows;
using System.Windows.Controls;
using CarRental.BackOffice.Services;

namespace CarRental.BackOffice.Pages;

// This page allows exporting data to CSV files
public partial class ExportCsvPage : Page
{
    private readonly DatabaseService _databaseService;

    public ExportCsvPage(DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
    }

    private void ExportButton_Click(object sender, RoutedEventArgs e)
    {
        var selectedItem = DataTypeComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem;
        if (selectedItem == null)
        {
            return;
        }

        string dataType = selectedItem.Content.ToString() ?? "";
        var csvService = new Services.CsvService(_databaseService);
        
        try
        {
            string filePath = csvService.ExportToCsv(dataType);
            StatusTextBlock.Text = $"Data exported successfully to:\n{filePath}";
            MessageBox.Show($"Data exported successfully to:\n{filePath}", "Export Complete", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            StatusTextBlock.Text = $"Error: {ex.Message}";
            MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


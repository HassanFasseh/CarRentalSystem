using System.Windows;
using Microsoft.Win32;
using CarRental.BackOffice.Services;

namespace CarRental.BackOffice.Windows;

// This window displays a QR code
public partial class QrCodeWindow : Window
{
    private readonly string _qrCodeText;

    public QrCodeWindow(string qrCodeText)
    {
        InitializeComponent();
        _qrCodeText = qrCodeText;
        
        // Generate and display QR code
        var qrService = new QrCodeService();
        QrCodeImage.Source = qrService.GenerateQrCode(_qrCodeText);
        InfoTextBlock.Text = $"QR Code for:\n{_qrCodeText}";
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
            Title = "Save QR Code",
            FileName = "QRCode.png"
        };

        if (saveFileDialog.ShowDialog() == true)
        {
            var qrService = new QrCodeService();
            qrService.SaveQrCodeToFile(_qrCodeText, saveFileDialog.FileName);
            MessageBox.Show("QR Code saved successfully.", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}


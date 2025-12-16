using System.IO;
using System.Windows.Media.Imaging;
using QRCoder;

namespace CarRental.BackOffice.Services;

// This service handles QR code generation
public class QrCodeService
{
    // This method generates a QR code image from text
    // Returns a BitmapImage that can be displayed in WPF
    public BitmapImage GenerateQrCode(string text)
    {
        // Create QR code generator
        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        
        // Create QR code as bitmap
        using PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);

        // Convert to BitmapImage for WPF
        BitmapImage bitmapImage = new BitmapImage();
        using (MemoryStream stream = new MemoryStream(qrCodeBytes))
        {
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = stream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
        }

        return bitmapImage;
    }

    // This method saves a QR code to a file
    public void SaveQrCodeToFile(string text, string filePath)
    {
        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        
        using PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = qrCode.GetGraphic(20);
        
        File.WriteAllBytes(filePath, qrCodeBytes);
    }
}


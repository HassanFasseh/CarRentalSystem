using System;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace CarRental.BackOffice.Services;

// This service handles sending emails from the BackOffice
// It uses SMTP to send email notifications
public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;

    // Constructor - reads email settings from a config file or uses defaults
    public EmailService()
    {
        // Try to read from a config file, or use defaults
        // For simplicity, we'll use a simple text file or environment variables
        // You can also hardcode for testing
        _smtpServer = GetSetting("SmtpServer", "smtp.gmail.com");
        _smtpPort = int.Parse(GetSetting("SmtpPort", "587"));
        _smtpUsername = GetSetting("SmtpUsername", "");
        _smtpPassword = GetSetting("SmtpPassword", "");
        _fromEmail = GetSetting("FromEmail", "noreply@carrental.com");
    }

    // This method reads a setting from a config file or returns default
    private string GetSetting(string key, string defaultValue)
    {
        try
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "email.config");
            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith(key + "="))
                    {
                        return line.Substring(key.Length + 1).Trim();
                    }
                }
            }
        }
        catch
        {
            // If config file doesn't exist or can't be read, use default
        }
        return defaultValue;
    }

    // This method sends an email when a rental is approved
    public async Task SendRentalApprovalAsync(string toEmail, string clientName, int rentalId, 
        string vehicleInfo, DateTime startDate, DateTime endDate, decimal totalAmount)
    {
        try
        {
            if (string.IsNullOrEmpty(_smtpUsername) || string.IsNullOrEmpty(_smtpPassword))
            {
                System.Diagnostics.Debug.WriteLine($"Approval email would be sent to {toEmail} for rental {rentalId} (SMTP not configured)");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Car Rental System", _fromEmail));
            message.To.Add(new MailboxAddress(clientName, toEmail));
            message.Subject = $"Rental Approved - Reservation #{rentalId}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2 style='color: #28a745;'>Rental Request Approved!</h2>
                        <p>Dear {clientName},</p>
                        <p>Great news! Your rental request has been <strong>approved</strong>.</p>
                        <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p><strong>Reservation ID:</strong> #{rentalId}</p>
                            <p><strong>Vehicle:</strong> {vehicleInfo}</p>
                            <p><strong>Start Date:</strong> {startDate:yyyy-MM-dd}</p>
                            <p><strong>End Date:</strong> {endDate:yyyy-MM-dd}</p>
                            <p><strong>Total Amount:</strong> <span style='color: #007bff; font-size: 18px;'>{totalAmount:C}</span></p>
                        </div>
                        <p>You can download your reservation PDF from your dashboard.</p>
                        <p>Thank you for choosing our car rental service!</p>
                    </body>
                    </html>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending approval email: {ex.Message}");
            // Don't throw - email failure shouldn't break the approval process
        }
    }

    // This method sends an email when a rental is denied
    public async Task SendRentalDenialAsync(string toEmail, string clientName, int rentalId, 
        string vehicleInfo, DateTime startDate, DateTime endDate)
    {
        try
        {
            if (string.IsNullOrEmpty(_smtpUsername) || string.IsNullOrEmpty(_smtpPassword))
            {
                System.Diagnostics.Debug.WriteLine($"Denial email would be sent to {toEmail} for rental {rentalId} (SMTP not configured)");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Car Rental System", _fromEmail));
            message.To.Add(new MailboxAddress(clientName, toEmail));
            message.Subject = $"Rental Request Update - Reservation #{rentalId}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif;'>
                        <h2 style='color: #dc3545;'>Rental Request Update</h2>
                        <p>Dear {clientName},</p>
                        <p>We regret to inform you that your rental request has been <strong>denied</strong>.</p>
                        <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                            <p><strong>Reservation ID:</strong> #{rentalId}</p>
                            <p><strong>Vehicle:</strong> {vehicleInfo}</p>
                            <p><strong>Requested Dates:</strong> {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}</p>
                        </div>
                        <p>If you have any questions, please contact our customer service.</p>
                        <p>Thank you for your interest in our car rental service.</p>
                    </body>
                    </html>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending denial email: {ex.Message}");
            // Don't throw - email failure shouldn't break the denial process
        }
    }
}


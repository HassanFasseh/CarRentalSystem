using System.Net;
using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;

namespace CarRental.Web.Services;

// This service handles sending emails
// It uses SMTP to send email confirmations
public class EmailService
{
    private readonly IConfiguration _configuration;

    // Constructor - receives configuration to get email settings
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method sends an email confirmation for a rental
    // It's called asynchronously so it doesn't block the web request
    public async Task SendRentalConfirmationAsync(string toEmail, string clientName, int rentalId, 
        string vehicleInfo, DateTime startDate, DateTime endDate, decimal totalAmount)
    {
        try
        {
            // Get email settings from configuration
            // For development, these can be set in appsettings.json
            string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            string smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            string smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
            string fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@carrental.com";

            // Create email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Car Rental System", fromEmail));
            message.To.Add(new MailboxAddress(clientName, toEmail));
            message.Subject = $"Rental Confirmation - Reservation #{rentalId}";

            // Create email body
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <html>
                    <body>
                        <h2>Rental Confirmation</h2>
                        <p>Dear {clientName},</p>
                        <p>Your rental request has been received and is being processed.</p>
                        <p><strong>Reservation ID:</strong> {rentalId}</p>
                        <p><strong>Vehicle:</strong> {vehicleInfo}</p>
                        <p><strong>Start Date:</strong> {startDate:yyyy-MM-dd}</p>
                        <p><strong>End Date:</strong> {endDate:yyyy-MM-dd}</p>
                        <p><strong>Total Amount:</strong> {totalAmount:C}</p>
                        <p>Thank you for choosing our car rental service!</p>
                    </body>
                    </html>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            // Send email using SMTP
            // Note: For production, you should use proper credentials
            // For development/testing, you can use a test SMTP server or skip email sending
            if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            else
            {
                // In development, just log that email would be sent
                // In a real application, you would configure proper SMTP settings
                System.Diagnostics.Debug.WriteLine($"Email would be sent to {toEmail} for rental {rentalId}");
            }
        }
        catch (Exception ex)
        {
            // Log error but don't fail the request
            // In production, you would log this to a file or logging service
            System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    // This method sends an email when a rental is approved
    public async Task SendRentalApprovalAsync(string toEmail, string clientName, int rentalId, 
        string vehicleInfo, DateTime startDate, DateTime endDate, decimal totalAmount)
    {
        try
        {
            string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            string smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            string smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
            string fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@carrental.com";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Car Rental System", fromEmail));
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

            if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Approval email would be sent to {toEmail} for rental {rentalId}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending approval email: {ex.Message}");
        }
    }

    // This method sends an email when a rental is denied
    public async Task SendRentalDenialAsync(string toEmail, string clientName, int rentalId, 
        string vehicleInfo, DateTime startDate, DateTime endDate)
    {
        try
        {
            string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            string smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            string smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
            string fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@carrental.com";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Car Rental System", fromEmail));
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

            if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
            {
                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Denial email would be sent to {toEmail} for rental {rentalId}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending denial email: {ex.Message}");
        }
    }
}


using System;
using System.IO;
using System.Linq;
using CarRental.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarRental.BackOffice.Services;

// This service handles PDF generation for reservations
public class PdfService
{
    private readonly DatabaseService _databaseService;

    public PdfService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        QuestPDF.Settings.License = LicenseType.Community; // Free license for community use
    }

    // This method generates a PDF reservation document
    public void GenerateReservationPdf(Rental rental)
    {
        using var context = _databaseService.GetContext();
        
        // Load full rental details
        var fullRental = context.Rentals
            .Include(r => r.Client)
            .ThenInclude(c => c!.User)
            .Include(r => r.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .Include(r => r.Employee)
            .FirstOrDefault(r => r.Id == rental.Id);

        if (fullRental == null)
        {
            throw new Exception("Rental not found.");
        }

        string fileName = $"Reservation_{fullRental.Id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Car Rental Reservation")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(20);

                        column.Item().Text($"Reservation ID: {fullRental.Id}").SemiBold();
                        column.Item().Text($"Date: {fullRental.CreatedAt:yyyy-MM-dd HH:mm}");

                        column.Item().Text("Client Information:").SemiBold();
                        column.Item().Text($"Name: {fullRental.Client?.FirstName} {fullRental.Client?.LastName}");
                        column.Item().Text($"Email: {fullRental.Client?.User?.Email ?? "N/A"}");
                        column.Item().Text($"Phone: {fullRental.Client?.PhoneNumber ?? "N/A"}");

                        column.Item().Text("Vehicle Information:").SemiBold();
                        column.Item().Text($"License Plate: {fullRental.Vehicle?.LicensePlate ?? "N/A"}");
                        column.Item().Text($"Make/Model: {fullRental.Vehicle?.Make} {fullRental.Vehicle?.Model}");
                        column.Item().Text($"Type: {fullRental.Vehicle?.VehicleType?.Name ?? "N/A"}");
                        column.Item().Text($"Year: {fullRental.Vehicle?.Year}");

                        column.Item().Text("Rental Details:").SemiBold();
                        column.Item().Text($"Start Date: {fullRental.StartDate:yyyy-MM-dd}");
                        column.Item().Text($"End Date: {fullRental.EndDate:yyyy-MM-dd}");
                        column.Item().Text($"Number of Days: {fullRental.NumberOfDays}");
                        column.Item().Text($"Daily Rate: {fullRental.DailyRate:C}");
                        column.Item().Text($"Total Amount: {fullRental.TotalAmount:C}").SemiBold();
                        column.Item().Text($"Status: {fullRental.Status}");

                        if (!string.IsNullOrWhiteSpace(fullRental.Notes))
                        {
                            column.Item().Text($"Notes: {fullRental.Notes}");
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Car Rental System - ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
            });
        })
        .GeneratePdf(filePath);

        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }
}


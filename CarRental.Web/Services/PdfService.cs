using System.IO;
using CarRental.Data;
using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CarRental.Web.Services;

// This service handles PDF generation for reservations
public class PdfService
{
    private readonly AppDbContext _context;

    // Constructor - receives database context
    public PdfService(AppDbContext context)
    {
        _context = context;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    // This method generates a PDF reservation document
    // Returns the PDF file as a byte array
    public byte[] GenerateReservationPdf(int rentalId)
    {
        // Load full rental details
        var rental = _context.Rentals
            .Include(r => r.Client)
            .ThenInclude(c => c!.User)
            .Include(r => r.Vehicle)
            .ThenInclude(v => v!.VehicleType)
            .Include(r => r.Employee)
            .FirstOrDefault(r => r.Id == rentalId);

        if (rental == null)
        {
            throw new Exception("Rental not found.");
        }

        // Generate PDF document
        byte[] pdfBytes = Document.Create(container =>
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

                        column.Item().Text($"Reservation ID: {rental.Id}").SemiBold();
                        column.Item().Text($"Date: {rental.CreatedAt:yyyy-MM-dd HH:mm}");

                        column.Item().Text("Client Information:").SemiBold();
                        column.Item().Text($"Name: {rental.Client?.FirstName} {rental.Client?.LastName}");
                        column.Item().Text($"Email: {rental.Client?.User?.Email ?? "N/A"}");
                        column.Item().Text($"Phone: {rental.Client?.PhoneNumber ?? "N/A"}");

                        column.Item().Text("Vehicle Information:").SemiBold();
                        column.Item().Text($"License Plate: {rental.Vehicle?.LicensePlate ?? "N/A"}");
                        column.Item().Text($"Make/Model: {rental.Vehicle?.Make} {rental.Vehicle?.Model}");
                        column.Item().Text($"Type: {rental.Vehicle?.VehicleType?.Name ?? "N/A"}");
                        column.Item().Text($"Year: {rental.Vehicle?.Year}");

                        column.Item().Text("Rental Details:").SemiBold();
                        column.Item().Text($"Start Date: {rental.StartDate:yyyy-MM-dd}");
                        column.Item().Text($"End Date: {rental.EndDate:yyyy-MM-dd}");
                        column.Item().Text($"Number of Days: {rental.NumberOfDays}");
                        column.Item().Text($"Daily Rate: {rental.DailyRate:C}");
                        column.Item().Text($"Total Amount: {rental.TotalAmount:C}").SemiBold();
                        column.Item().Text($"Status: {rental.Status}");

                        if (!string.IsNullOrWhiteSpace(rental.Notes))
                        {
                            column.Item().Text($"Notes: {rental.Notes}");
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
        .GeneratePdf();

        return pdfBytes;
    }
}


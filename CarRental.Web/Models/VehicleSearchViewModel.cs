namespace CarRental.Web.Models;

// This view model is used for searching and displaying vehicles
public class VehicleSearchViewModel
{
    public string? SearchTerm { get; set; }
    public int? VehicleTypeId { get; set; }
    public List<CarRental.Domain.Entities.Vehicle> Vehicles { get; set; } = new();
    public List<CarRental.Domain.Entities.VehicleType> VehicleTypes { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
    public int PageSize { get; set; } = 6;
}


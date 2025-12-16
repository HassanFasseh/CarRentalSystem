using System.ComponentModel.DataAnnotations;

namespace CarRental.Web.Models;

// This view model is used for requesting a rental
public class RentalRequestViewModel
{
    [Required]
    public int VehicleId { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [Display(Name = "End Date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public string? Notes { get; set; }
}


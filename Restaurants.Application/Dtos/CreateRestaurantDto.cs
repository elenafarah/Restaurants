using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Dtos;
public class CreateRestaurantDto
{
 //   [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
 //   [Required(ErrorMessage = "Please provide a Category!")]
    public string Category { get; set; } = null!;
    public bool HasDelivery { get; set; }

 //   [EmailAddress(ErrorMessage = "Please provide a valid Email!")]
    public string? ContactEmail { get; set; }

   // [Phone(ErrorMessage = "Please provide a valid Phone!")]
    public string? ContactNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }

   // [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Please provide a valid Postal Code XX-XXX")]
    public string? PostalCode { get; set; }
}


using MediatR;

namespace Restaurants.Application.Dishes.Commands;
public class PatchDishCommand : IRequest
{
    public int RestaurantId { get; set; }
    public int DishId { get; set; }

    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal? Price { get; init; }
    public int? KiloCalories { get; init; }
}


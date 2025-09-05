using MediatR;

namespace Restaurants.Application.Dishes.Commands;

public class UpdateDishCommand: IRequest
{
    public int RestaurantId { get; set; }
    public int DishId { get; set; }
    public string Name { get; init; } = default!;
    public string Description { get; init; } = default!;
    public decimal Price { get; init; }
    public int? KiloCalories { get; init; }
}


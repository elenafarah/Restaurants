using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }

    public int? KiloCalories { get; set; }

    public static DishDto FromEntity(Dish entity)
    {
        return new DishDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            KiloCalories = entity.KiloCalories
        };
    }
}


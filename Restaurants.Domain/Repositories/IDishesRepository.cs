using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;
public interface IDishesRepository
{
    public Task<IEnumerable<Dish>> GetAllAsync(int RestaurantId);
    public Task<int> Create(Dish entity);
    Task Delete(Dish dish, CancellationToken ct);

    Task<Dish?> GetByIdAsync(int restaurantId, int dishId, CancellationToken ct);
    Task Update(Dish dish, CancellationToken ct);
    Task Patch(Dish dish, CancellationToken ct);
}


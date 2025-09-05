using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;
public class DishesRepository (RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<IEnumerable<Dish>> GetAllAsync(int RestaurantId)
    {
        throw new NotImplementedException();
    }

    public async Task<Dish?> GetByIdAsync(int restaurantId, int dishId, CancellationToken ct)
        => await dbContext.Dishes
            .FirstOrDefaultAsync(d => d.RestaurantId == restaurantId && d.Id == dishId, ct);

    public async Task<int> Create(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Dish dish, CancellationToken ct)
        => await dbContext.SaveChangesAsync(ct);

    public async Task Patch(Dish dish, CancellationToken ct)
        => await dbContext.SaveChangesAsync(ct);

    public async Task Delete(Dish dish, CancellationToken ct)
    {
        dbContext.Dishes.Remove(dish);
        await dbContext.SaveChangesAsync(ct);
    }
}


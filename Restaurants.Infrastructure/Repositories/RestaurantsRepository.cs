using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;
public class RestaurantsRepository(RestaurantsDbContext dbContext): IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var restaurants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
       

        return restaurants; 
    }

    public async Task<Restaurant?> GetById(int id)
    {
        var restaurant = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(x => x.Id == id);

        return restaurant;
    }

    public async Task<int> Create(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);

        await dbContext.SaveChangesAsync();

        return restaurant.Id;
    }

    public async Task DeleteRestaurant(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task Patch()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase, CancellationToken ct = default)
    {
        IQueryable<Restaurant> query = dbContext.Restaurants
            .Include(r => r.Dishes)      
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchPhrase))
        {
            var term = $"%{searchPhrase.Trim()}%";

            query = query.Where(r =>
                 EF.Functions.Like(EF.Functions.Collate(r.Name, "Latin1_General_CI_AI"), term) ||
                 EF.Functions.Like(EF.Functions.Collate(r.Description, "Latin1_General_CI_AI"), term));
        }

        return await query.ToListAsync(ct);
    }

    public async Task<(IReadOnlyList<Restaurant> Items, int TotalCount)> GetAllMatchingAsync(
        string? searchPhrase, 
        int pageNumber, 
        int pageSize,
        string? sortBy,
        SortDirection sortDirection,
        CancellationToken ct)
    {
        string? searchLower = searchPhrase?.ToLower();

        // aussi si on souhaite rajouter des validations supplémentaires
        const int MaxPageSize = 100;             
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, MaxPageSize);


        IQueryable<Restaurant> query = dbContext.Restaurants
            .Include(r => r.Dishes)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchLower))
        {
            var term = $"%{searchLower.Trim()}%";

            query = query.Where(r =>
                EF.Functions.Like(EF.Functions.Collate(r.Name, "Latin1_General_CI_AI"), term) ||
                EF.Functions.Like(EF.Functions.Collate(r.Description, "Latin1_General_CI_AI"), term));
        }


        var total = await query.CountAsync(ct);

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var columns = new Dictionary<string, Expression<Func<Restaurant, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(Restaurant.Name)] = r => r.Name!,
                [nameof(Restaurant.Description)] = r => r.Description!,
                [nameof(Restaurant.Category)] = r => r.Category!
            };

            if (columns.TryGetValue(sortBy, out var keySelector))
            {
                query = sortDirection == SortDirection.Desc
                    ? query.OrderByDescending(keySelector)
                    : query.OrderBy(keySelector);
            }
        }

        var toSkip = (pageNumber - 1) * pageSize;

        var items = await query
            .Skip(toSkip)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, total);
    }

}


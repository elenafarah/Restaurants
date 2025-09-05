using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;
public interface IRestaurantsRepository
{
    public Task<IEnumerable<Restaurant>> GetAllAsync();
    public Task<Restaurant?> GetById(int id);

    public Task<int> Create(Restaurant restaurant);
    public Task DeleteRestaurant(Restaurant restaurant);
    public Task Patch();
    public Task Update();
   // Task<IEnumerable<Restaurant>> GetAllMatchingAsync(string? searchPhrase, CancellationToken ct);

    Task<(IReadOnlyList<Restaurant> Items, int TotalCount)> GetAllMatchingAsync(
        string? searchPhrase, 
        int pageNumber, 
        int pageSize,
        string? sortBy,
        SortDirection sortDirection,
        CancellationToken ct
        );
}


using Restaurants.Application.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    public Task<IEnumerable<RestaurantDto>> GetAllRestaurants();

    public Task<RestaurantDto?> GetById(int id);

    public Task<int> Create(CreateRestaurantDto dto);
}


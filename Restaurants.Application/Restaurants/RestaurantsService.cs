using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;
public class RestaurantsService(
    IRestaurantsRepository restaurantsRepository, 
    ILogger<RestaurantsService> logger,
    IMapper mapper
    ) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all information");

        var restaurants = await restaurantsRepository.GetAllAsync();
        // var restaurantsDto = restaurants.Select(RestaurantDto.FromEntity);
       // return restaurantsDto!; 
       return mapper.Map<IEnumerable<RestaurantDto>>(restaurants)!;
    }


    public async Task<RestaurantDto?> GetById(int id)
    {
        logger.LogInformation($"Getting an Restaurant information by {id}");

        var restaurant = await restaurantsRepository.GetById(id);

        // var restaurantDto = RestaurantDto.FromEntity(restaurant);
        // return restaurantDto;
        return mapper.Map<RestaurantDto?>(restaurant);
    }

    public async Task<int> Create(CreateRestaurantDto dto)
    {
        logger.LogInformation("Create a new restaurant");

        var restaurant = mapper.Map<Restaurant>(dto);

        var restaurantId = await restaurantsRepository.Create(restaurant);

        return restaurantId;

    }
}


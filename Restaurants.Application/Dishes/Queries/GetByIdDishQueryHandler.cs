using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants.Queries;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries;

public class GetByIdDishQueryHandler (
    ILogger<GetByIdDishQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository
) : IRequestHandler<GetByIdDishQuery, DishDto>
{
    public async Task<DishDto> Handle(GetByIdDishQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting the dish of restaurantId : {restaurantId} and dishId : {dishId}", request.RestaurantId, request.DishId);

        var restaurant = await restaurantsRepository.GetById(request.RestaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId);

        if (dish == null) throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        return mapper.Map<DishDto>(dish); 
    }
}


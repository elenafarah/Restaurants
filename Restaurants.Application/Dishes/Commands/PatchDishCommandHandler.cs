using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands;

public class PatchDishCommandHandler(
    ILogger<PatchDishCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository
) : IRequestHandler<PatchDishCommand>
{
    public async Task Handle(PatchDishCommand request, CancellationToken ct)
    {
        var restaurant = await restaurantsRepository.GetById(request.RestaurantId);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dish = await dishesRepository.GetByIdAsync(request.RestaurantId, request.DishId, ct);

        if (dish is null)
        {
            logger.LogInformation("Dish {DishId} not found for restaurant {RestaurantId}.",
                request.DishId, request.RestaurantId);
            throw new NotFoundException(nameof(Dish), request.RestaurantId.ToString());
        }

        mapper.Map(request, dish);

        await dishesRepository.Patch(dish, ct);
        
    }
}


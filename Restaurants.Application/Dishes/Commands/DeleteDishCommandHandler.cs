using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands;

public sealed class DeleteDishCommandHandler(
    ILogger<DeleteDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository
) : IRequestHandler<DeleteDishCommand>
{
    public async Task Handle(DeleteDishCommand request, CancellationToken ct)
    {

        var restaurant = await restaurantsRepository.GetById(request.RestaurantId);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dish = await dishesRepository.GetByIdAsync(request.RestaurantId, request.DishId, ct);

        if (dish is null)
        {
            logger.LogInformation("Dish {DishId} not found for restaurant {RestaurantId}.", request.DishId, request.RestaurantId);

            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }

        await dishesRepository.Delete(dish, ct);

    }
}


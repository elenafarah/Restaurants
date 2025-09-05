using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Commands;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands;
public class CreateDishCommandHandler(
    ILogger<CreateDishCommandHandler> logger,
    IMapper mapper,
    IDishesRepository dishesRepository,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new dish {@DishRequest}", request);

        var restaurant = await restaurantsRepository.GetById(request.RestaurantId);

        if (restaurant == null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dish = mapper.Map<Dish>(request);

       return  await dishesRepository.Create(dish);

    }
}




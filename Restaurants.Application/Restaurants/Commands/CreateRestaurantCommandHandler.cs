using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands;
public class CreateRestaurantCommandHandler
(
    ILogger<CreateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IUserContext userContext
    ) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserEmail} with {UserId} creates a new restaurant {@Restaurant}", currentUser.Email, currentUser.Id, request);

        var restaurant = mapper.Map<Restaurant>(request);

        restaurant.OwnerId = currentUser.Id;

        var restaurantId = await restaurantsRepository.Create(restaurant);

        return restaurantId;
    }
}


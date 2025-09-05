using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands;

public class UpdateRestaurantCommandHandler (
    ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService authz
) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Update a restaurant");
        var restaurant = await restaurantsRepository.GetById(request.Id);
        if (restaurant is null)
            throw new NotFoundException("Restaurant", $"{request.Id}");

        if (!authz.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException("Vous n’êtes pas autorisé à modifier ce restaurant.");

        mapper.Map(request, restaurant);

        await restaurantsRepository.Update();
    }
}


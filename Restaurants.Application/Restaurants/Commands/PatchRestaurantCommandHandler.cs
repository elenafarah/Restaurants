using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands;

public class PatchRestaurantCommandHandler (
    ILogger<PatchRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService authz
) : IRequestHandler<PatchRestaurantCommand>
{
    public async Task Handle(PatchRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Patch a restaurant id: {RestaurantId} with {@PatchRestaurantCommand}", request.Id, request);

        var restaurant = await restaurantsRepository.GetById(request.Id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant", $"{request.Id}");

        if (!authz.Authorize(restaurant, ResourceOperation.Patch))
            throw new ForbidException("Vous n’êtes pas autorisé à modifier ce restaurant.");

        mapper.Map(request, restaurant);
        await restaurantsRepository.Patch();
    }
}


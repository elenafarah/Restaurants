using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands;
public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService authz
) : IRequestHandler<DeleteRestaurantCommand>
{
    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting Restaurant with id : {RestaurantId}", request.Id);

        var restaurant = await restaurantsRepository.GetById(request.Id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant", $"{request.Id}");

        if(!authz.Authorize(restaurant, ResourceOperation.Delete))
            throw new ForbidException("Vous n’êtes pas autorisé à supprimer ce restaurant.");

        await restaurantsRepository.DeleteRestaurant(restaurant);
  }
}


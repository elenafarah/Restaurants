using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands;

public class UploadRestaurantLogoCommandHandler (
    ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService authz, 
    IBlobStorageService blobStorageService
    ) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Update a logo restaurant by UploadRestaurantLogoCommandHandler id {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetById(request.RestaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant", $"{request.RestaurantId}");

        if (!authz.Authorize(restaurant, ResourceOperation.Update))
            throw new ForbidException("Vous n’êtes pas autorisé à modifier ce restaurant.");

        var logoUrl = await blobStorageService.UploadToBlobAsync(request.FileName, request.File);

        restaurant.LogoUrl = logoUrl;

        await restaurantsRepository.Update();
    }
}


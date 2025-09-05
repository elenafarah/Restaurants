using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Repositories;

public sealed class RestaurantAuthorizationService(
    ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext
) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation operation)
    {
        logger.LogInformation("Authorizing user {UserId} for {Op} on restaurant {Name}", userContext.GetCurrentUser(), operation, restaurant.Name);

        
        if (operation is ResourceOperation.Create or ResourceOperation.Read)
            return true;

        var isOwner = userContext.GetCurrentUser()?.Id is not null && restaurant.OwnerId == userContext.GetCurrentUser()?.Id;

        var isAdmin = userContext.GetCurrentUser()?.IsInRole("Admin") ?? false;

        if ((operation is ResourceOperation.Update) || (operation is ResourceOperation.Patch))
            return isOwner;

        if (operation is ResourceOperation.Delete)
            return isOwner || isAdmin;

        return false;
    }
}


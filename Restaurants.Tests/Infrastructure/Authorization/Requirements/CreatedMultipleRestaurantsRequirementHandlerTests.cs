using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Domain;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Authorization.Requirements;
using Xunit;

namespace Restaurants.Tests.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedEnoughRestaurants_Succeeds()
    {
        // Arrange
        var userId = "user-1";
        var currentUser = new CurrentUser(userId, "test@email.com", new List<string>(), null, null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new Restaurant { Id = 1, OwnerId = userId },
            new Restaurant { Id = 2, OwnerId = userId },
            new Restaurant { Id = 3, OwnerId = "other-user" }
        };

        var repoMock = new Mock<IRestaurantsRepository>();
        repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(repoMock.Object, userContextMock.Object);

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) })),
            null
        );

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedEnoughRestaurants_Fails()
    {
        // Arrange
        var userId = "user-2";
        var currentUser = new CurrentUser(userId, "test2@email.com", new List<string>(), null, null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new Restaurant { Id = 1, OwnerId = "other-user" }
        };

        var repoMock = new Mock<IRestaurantsRepository>();
        repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(1);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(repoMock.Object, userContextMock.Object);

        var context = new AuthorizationHandlerContext(
            new[] { requirement },
            new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) })),
            null
        );

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
        Assert.True(context.HasFailed);
    }
}

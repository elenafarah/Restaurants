using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Tests.Application.Restaurants.Commands
{
    public class CreateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<CreateRestaurantCommandHandler>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IRestaurantsRepository> _repositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        private readonly CreateRestaurantCommandHandler _handler;

        public CreateRestaurantCommandHandlerTests()
        {
            _handler = new CreateRestaurantCommandHandler(
                _loggerMock.Object,
                _mapperMock.Object,
                _repositoryMock.Object,
                _userContextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateRestaurantAndReturnId()
        {
            // Arrange
            var user = new CurrentUser("user-1", "test@email.com", new[] { "User" }, null, null);
            var command = new CreateRestaurantCommand
            {
                Name = "Test",
                Description = "Desc",
                Category = "Cat",
                HasDelivery = true,
                ContactEmail = "contact@email.com",
                ContactNumber = "123456789",
                City = "Paris",
                Street = "1 rue de Paris",
                PostalCode = "75000"
            };
            var restaurant = new Restaurant();
            _userContextMock.Setup(x => x.GetCurrentUser()).Returns(user);
            _mapperMock.Setup(x => x.Map<Restaurant>(command)).Returns(restaurant);
            _repositoryMock.Setup(x => x.Create(restaurant)).ReturnsAsync(42);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(42, result);
            Assert.Equal(user.Id, restaurant.OwnerId);
            _repositoryMock.Verify(x => x.Create(restaurant), Times.Once);

            // Vérification du logging via It.IsAny<> car LogInformation est une méthode d'extension
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("creates a new restaurant")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once
            );
        }
    }
}

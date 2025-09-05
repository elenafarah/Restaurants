using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants.Queries;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using Xunit;

namespace Restaurants.Tests.Api.Controllers
{
    public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository),
                        _ => _restaurantsRepositoryMock.Object));

                });
            });
        }

        [Fact]
        public async Task GetAll_ShouldReturnPagedResultOfRestaurants()
        {
            // Arrange
            var url = "/api/restaurants?pageNumber=1&pageSize=10";
            var _client = _factory.CreateClient();
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pagedResult = await response.Content.ReadFromJsonAsync<PagedResult<RestaurantDto>>();
            Assert.NotNull(pagedResult);
            Assert.True(pagedResult.PageNumber == 1);
            Assert.True(pagedResult.PageSize == 10);
            Assert.NotNull(pagedResult.Items);
        }

        [Fact]
        public async Task GetById_ShouldReturn404NotFound_WhenNonExistingId()
        {
            // Arrange
            var id = 1123; 
            var url = $"/api/restaurants/{id}";
            _restaurantsRepositoryMock.Setup(x => x.GetById(id))
                .ReturnsAsync((Restaurant?)null);

            var _client = _factory.CreateClient();
            // Act
            var response = await _client.GetAsync(url);
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturn200Ok_WhenExistingId()
        {
            // arrange

            var id = 99;

            var restaurant = new Restaurant()
            {
                Id = id,
                Name = "Test",
                Description = "Test description"
            };

            _restaurantsRepositoryMock.Setup(m => m.GetById(id)).ReturnsAsync(restaurant);

            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");

            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Name.Should().Be("Test");
            restaurantDto.Description.Should().Be("Test description");
        }
    }
}

using AutoMapper;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants.Commands;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Tests.Application.Dtos
{
    public class RestaurantsProfileTests
    {
        private readonly IMapper _mapper;

        public RestaurantsProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_CreateRestaurantDto_To_CreateRestaurantCommand()
        {
            var dto = new CreateRestaurantDto
            {
                Name = "Test",
                City = "Paris",
                Street = "Rue de Test",
                PostalCode = "75000"
            };

            var command = _mapper.Map<CreateRestaurantCommand>(dto);

            Assert.Equal(dto.Name, command.Name);
            Assert.Equal(dto.City, command.City);
            Assert.Equal(dto.Street, command.Street);
            Assert.Equal(dto.PostalCode, command.PostalCode);
        }

        [Fact]
        public void Should_Map_CreateRestaurantCommand_To_Restaurant()
        {
            var command = new CreateRestaurantCommand
            {
                Name = "Test",
                City = "Paris",
                Street = "Rue de Test",
                PostalCode = "75000"
            };

            var restaurant = _mapper.Map<Restaurant>(command);

            Assert.Equal(command.Name, restaurant.Name);
            Assert.NotNull(restaurant.Address);
            Assert.Equal(command.City, restaurant.Address.City);
            Assert.Equal(command.Street, restaurant.Address.Street);
            Assert.Equal(command.PostalCode, restaurant.Address.PostalCode);
        }

        [Fact]
        public void Should_Map_Restaurant_To_RestaurantDto()
        {
            var restaurant = new Restaurant
            {
                Name = "Test",
                Address = new Address
                {
                    City = "Paris",
                    Street = "Rue de Test",
                    PostalCode = "75000"
                },
                Dishes = new List<Dish>()
            };

            var dto = _mapper.Map<RestaurantDto>(restaurant);

            Assert.Equal(restaurant.Name, dto.Name);
            Assert.Equal(restaurant.Address.City, dto.City);
            Assert.Equal(restaurant.Address.Street, dto.Street);
            Assert.Equal(restaurant.Address.PostalCode, dto.PostalCode);
            Assert.NotNull(dto.Dishes);
        }
    }
}

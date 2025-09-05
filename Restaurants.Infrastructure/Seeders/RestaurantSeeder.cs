using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;
public class RestaurantSeeder(RestaurantsDbContext dbContext): IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        User owner = new User()
        {
            Email = "seed-user@test.com"
        };

        List<Restaurant> restaurants = [
            new ()
            {
                Owner = owner,
                Name = "KFC",
                Category = "Fast Food",
                Description = "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Address = new Address()
                {
                    Street = "Cork st 5",
                    City = "London",
                    PostalCode = "W2CN 5DU"
                },
                Dishes = [
                    new Dish()
                {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets 5pcs.",
                        Price = 5.30M

                },
                    new Dish()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken 10 pcs.",
                        Price = 10.30M
                        
                    }
                ]
            },
            new ()
            {
                Owner = owner,
                Name = "McDonald",
                Category = "Fast Food",
                Description = "MCDonald Corporation, incorporated on December 21 1964",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 193",
                    PostalCode = "WFI 8SR"
                }
            }
        ];


        return restaurants;
    }
}


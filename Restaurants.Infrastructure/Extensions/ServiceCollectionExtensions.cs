using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirements;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RestaurantsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("RestaurantsDb"))
                .EnableSensitiveDataLogging());

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();

        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddAuthorizationBuilder()

            .AddPolicy(PolicyNames.HasNationality, policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim(AppClaimTypes.Nationality, "American", "Polish"))

            .AddPolicy(PolicyNames.AtLeast20, policy =>
                policy.RequireAuthenticatedUser()
                    .AddRequirements(new MinimumAgeRequirement(20)))

            .AddPolicy(PolicyNames.CreatedAtLeastTwoRestaurants, policy =>
                policy.RequireAuthenticatedUser()
                    .AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();

        services.Configure<BlobStorageSettings>(configuration.GetSection("BlobStorage"));

        services.AddScoped<IBlobStorageService, BlobStorageService>();

        return services;
    }
}


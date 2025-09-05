using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Commands;
using Restaurants.Application.Users;
using Restaurants.Application.Validators;



namespace Restaurants.Application.Extensions;
public static class ServiceCollectionsExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionsExtension).Assembly;

        services.AddScoped<IRestaurantsService, RestaurantsService>();

        services.AddAutoMapper(typeof(RestaurantsProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<CreateRestaurantDtoValidator>();

        services.AddValidatorsFromAssemblyContaining<CreateRestaurantCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateRestaurantCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<PatchRestaurantCommandValidator>();

        services.AddFluentValidationAutoValidation(options =>
        {
            options.DisableDataAnnotationsValidation = true;
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

         
        services.AddScoped<IUserContext, UserContext>();

        return services;

    }

}


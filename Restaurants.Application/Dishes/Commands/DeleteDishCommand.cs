using MediatR;

namespace Restaurants.Application.Dishes.Commands;
public sealed record DeleteDishCommand(int RestaurantId, int DishId) : IRequest;


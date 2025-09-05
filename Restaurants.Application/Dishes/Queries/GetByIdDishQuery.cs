using MediatR;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Dishes.Queries;

public sealed record GetByIdDishQuery (int RestaurantId, int DishId) : IRequest<DishDto>;


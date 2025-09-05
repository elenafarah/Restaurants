using MediatR;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Dishes.Queries;

public sealed record GetAllDishesQuery (int RestaurantId) : IRequest<IEnumerable<DishDto>>;



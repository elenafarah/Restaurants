
using MediatR;

namespace Restaurants.Application.Restaurants.Commands;

public sealed record DeleteRestaurantCommand(int Id) : IRequest;



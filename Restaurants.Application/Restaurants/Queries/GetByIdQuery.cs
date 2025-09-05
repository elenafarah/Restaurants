using MediatR;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Restaurants.Queries;

public sealed record GetByIdQuery(int Id) : IRequest<RestaurantDto>;

/*

public class GetByIdQuery : IRequest<RestaurantDto?>
{

    public int Id { get; set; }
}

 
*/
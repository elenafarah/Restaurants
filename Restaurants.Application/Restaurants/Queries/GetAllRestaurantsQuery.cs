using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Queries;

public sealed class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchPhrase { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    // ⬇️ tri
    public string? SortBy { get; init; }             // "Name" | "Description" | "Category"
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}




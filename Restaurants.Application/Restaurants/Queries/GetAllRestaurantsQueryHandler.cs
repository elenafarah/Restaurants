using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries;
public class GetAllRestaurantsQueryHandler
    (
        ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper,
        IRestaurantsRepository restaurantsRepository
    ) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken ct)
    {
        logger.LogInformation("Getting all restaurants");

        var (restaurants, total) = await restaurantsRepository.GetAllMatchingAsync(
            request.SearchPhrase,
            request.PageNumber,
            request.PageSize,
            request.SortBy,
            request.SortDirection,
            ct);

        var items = mapper.Map<IReadOnlyList<RestaurantDto>>(restaurants)!;

        //  var restaurants = await restaurantsRepository.GetAllAsync();
        //   var restaurants = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase, ct);
        // return mapper.Map<IEnumerable<RestaurantDto>>(restaurants)!;

        return new PagedResult<RestaurantDto>(
            items, 
            total, 
            request.PageNumber, 
            request.PageSize
            );
    }
}


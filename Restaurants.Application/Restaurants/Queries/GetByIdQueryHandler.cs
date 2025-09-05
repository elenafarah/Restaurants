using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries;
public class GetByIdQueryHandler(
    ILogger<GetByIdQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository, 
    IBlobStorageService blobStorage
) : IRequestHandler<GetByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting an Restaurant information by {RestaurantId}", request.Id);
        var restaurant = 
            await restaurantsRepository.GetById(request.Id) 
            ?? 
            throw new NotFoundException("Restaurant",$"{request.Id}");

        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

        restaurantDto.LogoSasUrl = blobStorage.GetBlobSasUrl(restaurant.LogoUrl);

        return restaurantDto;
    }
}


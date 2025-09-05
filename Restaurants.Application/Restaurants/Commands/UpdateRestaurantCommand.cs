using MediatR;

namespace Restaurants.Application.Restaurants.Commands;
public sealed record UpdateRestaurantCommand(
    int Id,
    string Name,
    string Category,
    string Description,
    string ContactEmail,
    string ContactNumber,
    string City,
    string Street,
    string PostalCode,
    bool HasDelivery
    ) : IRequest;




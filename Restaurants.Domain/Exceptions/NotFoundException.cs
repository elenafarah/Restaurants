namespace Restaurants.Domain.Exceptions;

public sealed class NotFoundException(string resouceType, string resourceIdentifier): Exception($"{resouceType} with id: {resourceIdentifier} doesn't exist")
{

}


namespace Restaurants.Domain.Exceptions;

public sealed class ForbidException(string? message = null) : Exception(message)
{
}

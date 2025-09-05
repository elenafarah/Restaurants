using MediatR;

namespace Restaurants.Application.Users.Commads.UnassignUserRole;

public sealed record UnassignUserRoleCommand(string Email, string RoleName) : IRequest;


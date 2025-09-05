using MediatR;

namespace Restaurants.Application.Users.Commads.AssignUserRole;

public sealed record AssignUserRoleCommand(string Email, string RoleName) : IRequest;


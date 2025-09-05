using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commads.AssignUserRole;

public sealed class AssignUserRoleCommandHandler (
    ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assign user '{Email}' to role '{RoleName}'", request.Email, request.RoleName);

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new NotFoundException(nameof(User), request.Email);


        var role = await roleManager.FindByNameAsync(request.RoleName);

        if (role is null)
            throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        var result = await userManager.AddToRoleAsync(user, role.Name!);


    }
}


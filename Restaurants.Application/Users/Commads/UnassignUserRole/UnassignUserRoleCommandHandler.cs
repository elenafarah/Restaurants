using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commads.UnassignUserRole;

public sealed class UnassignUserRoleCommandHandler(
    ILogger<UnassignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager
) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken ct)
    {
        logger.LogInformation("Unassign user '{Email}' from role '{RoleName}'", request.Email, request.RoleName);

        // 1) Vérifier l’utilisateur
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new NotFoundException("User", request.Email);

        // 2) Vérifier le rôle (FindByNameAsync dépend de NormalizedName en BDD)
        var role = await roleManager.FindByNameAsync(request.RoleName);
        if (role is null)
            throw new NotFoundException("IdentityRole", request.RoleName);

        // 3) Si l’utilisateur n’est pas dans ce rôle, on considère l’opération idempotente
        if (!await userManager.IsInRoleAsync(user, role.Name!))
        {
            logger.LogInformation("User '{Email}' n'est pas dans le rôle '{RoleName}', rien à retirer.", request.Email, request.RoleName);
            return; // 204 No Content côté API (idempotent)
        }

        // 4) Retirer le rôle
        var result = await userManager.RemoveFromRoleAsync(user, role.Name!);
        if (!result.Succeeded)
        {
            var msg = string.Join("; ", result.Errors.Select(e => $"{e.Code}:{e.Description}"));
            throw new ValidationException($"Retrait du rôle échoué : {msg}");
        }
    }
}


using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Seeders;

public static class IdentitySeeder
{
    private static readonly string[] Roles = ["Admin", "Owner", "User", "Manager", "Auditor"];

    public static async Task SeedAsync(IServiceProvider sp, CancellationToken ct = default)
    {
        using var scope = sp.CreateScope();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var name in Roles)
        {
            if (!await roleMgr.RoleExistsAsync(name))
            {
                var res = await roleMgr.CreateAsync(new IdentityRole(name));
                if (!res.Succeeded)
                {
                    var msg = string.Join("; ", res.Errors.Select(e => $"{e.Code}:{e.Description}"));
                    throw new Exception($"Role '{name}' creation failed: {msg}");
                }
            }
        }

        var users = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        var attributions = new (string Email, string[] Roles)[]
        {
            ("admin@test.com", new[] { "Admin", "User" }),
            ("owner@test.com", new[] { "Owner" }),
            ("test@test.com", new[] { "User" }),
            ("testuser@test.com", new[] { "User" })
        };


        foreach (var (email, roleList) in attributions)
        {
            var user = await users.FindByEmailAsync(email);
            if (user is null) { continue; }

            foreach (var roleName in roleList.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!await roleMgr.RoleExistsAsync(roleName))
                {
                    var createRole = await roleMgr.CreateAsync(new IdentityRole(roleName));
                    if (!createRole.Succeeded)
                    {
                        var msg = string.Join("; ", createRole.Errors.Select(e => $"{e.Code}:{e.Description}"));
                        throw new Exception($"Création du rôle '{roleName}' échouée : {msg}");
                    }
                }

                if (!await users.IsInRoleAsync(user, roleName))
                {
                    var res = await users.AddToRoleAsync(user, roleName);
                    if (!res.Succeeded)
                    {
                        var msg = string.Join("; ", res.Errors.Select(e => $"{e.Code}:{e.Description}"));
                        throw new Exception($"Assignation du rôle '{roleName}' à '{email}' échouée : {msg}");
                    }
                }
            }
        }
    }


}

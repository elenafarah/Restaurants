using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commads.AssignUserRole;
using Restaurants.Application.Users.Commads.UnassignUserRole;
using Restaurants.Application.Users.Commads.UpdateUserDetails;
using Restaurants.Domain.Entities;

namespace Restaurants.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IdentityController (IMediator mediator, UserManager<User> userManager) : ControllerBase
{
    [HttpPatch("user")]
    [Authorize]
    public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
    {
        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("user-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleCommand command)
    {
        await mediator.Send(command);

        return NoContent();
    }

    [HttpPost("user-role/unassign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unassign([FromBody] UnassignUserRoleCommand command, CancellationToken ct)
    {
        await mediator.Send(command, ct);
        return NoContent();
    }

    [HttpGet("me/roles")]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<string>>> GetMyRoles(CancellationToken ct)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Unauthorized();

        var roles = await userManager.GetRolesAsync(user);
        return Ok(roles?.ToList() ?? []);
    }
}


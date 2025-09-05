using FluentValidation;

namespace Restaurants.Application.Users.Commads.AssignUserRole;

public sealed class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand>
{
    public AssignUserRoleCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email requis.")
            .EmailAddress().WithMessage("Email invalide.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName requis.")
            .MaximumLength(256);
    }
}

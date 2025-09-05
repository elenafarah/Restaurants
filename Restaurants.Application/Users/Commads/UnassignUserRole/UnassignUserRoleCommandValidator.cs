using FluentValidation;

namespace Restaurants.Application.Users.Commads.UnassignUserRole;

public sealed class UnassignUserRoleCommandValidator : AbstractValidator<UnassignUserRoleCommand>
{
    public UnassignUserRoleCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email requis.")
            .EmailAddress().WithMessage("Email invalide.");

        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName requis.")
            .MaximumLength(256);
    }
}
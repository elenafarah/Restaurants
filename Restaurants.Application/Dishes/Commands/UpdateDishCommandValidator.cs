using FluentValidation;

namespace Restaurants.Application.Dishes.Commands;

public sealed class UpdateDishCommandValidator : AbstractValidator<UpdateDishCommand>
{
    public UpdateDishCommandValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom est obligatoire.")
            .Length(3, 100).WithMessage("Le nom doit contenir entre 3 et 100 caractères.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La description est obligatoire.");

        RuleFor(x => x.Price)
            .GreaterThan(0m).WithMessage("Le prix doit être > 0.");

        When(x => x.KiloCalories.HasValue, () =>
        {
            RuleFor(x => x.KiloCalories!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Les calories doivent être >= 0.");
        });
    }
}


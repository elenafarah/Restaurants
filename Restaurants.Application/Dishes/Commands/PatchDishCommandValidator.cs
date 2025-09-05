using FluentValidation;

namespace Restaurants.Application.Dishes.Commands;

public sealed class PatchDishCommandValidator : AbstractValidator<PatchDishCommand>
{
    public PatchDishCommandValidator()
    {
        // Au moins un champ fourni
        RuleFor(x => x)
            .Must(x => x.Name != null
                       || x.Description != null
                       || x.Price.HasValue
                       || x.KiloCalories.HasValue)
            .WithMessage("Au moins un champ doit être fourni.");

        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name!)
                .NotEmpty().WithMessage("Le nom ne peut pas être vide.")
                .Length(3, 100).WithMessage("Le nom doit contenir entre 3 et 100 caractères.");
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(x => x.Description!)
                .NotEmpty().WithMessage("La description ne peut pas être vide.");
        });

        When(x => x.Price.HasValue, () =>
        {
            RuleFor(x => x.Price!.Value)
                .GreaterThan(0m).WithMessage("Le prix doit être > 0.");
        });

        When(x => x.KiloCalories.HasValue, () =>
        {
            RuleFor(x => x.KiloCalories!.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Les calories doivent être >= 0.");
        });
    }
}


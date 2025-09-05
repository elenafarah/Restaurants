using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands;

public class PatchRestaurantCommandValidator : AbstractValidator<PatchRestaurantCommand>
{
    public PatchRestaurantCommandValidator()
    {
        RuleFor(x => x.Name!)
            .Length(3, 100)
            .WithMessage("Le nom doit contenir entre 3 et 100 caractères.");

        When(x => x.Description is not null, () =>
        {
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description ne peut pas être null");
        }); 



    }
}


using FluentValidation;

namespace Restaurants.Application.Dishes.Commands;
public class CreateDishCommandValidator: AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dish => dish.Name)
            .NotEmpty()
            .WithMessage("Give a Name for Dish");

        RuleFor(dish => dish.Description)
            .NotEmpty()
            .WithMessage("Give a Description for Dish");

        RuleFor(dish => dish.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");

        RuleFor(dish => dish.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a non-negative number.");
    }
}


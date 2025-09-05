using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands;

public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "Indian"];

    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("L'identifiant est requis.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom est obligatoire.")
            .Length(3, 100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Please provide a Description.");

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is Required.")
            .Custom((value, context) =>
            {
                if (!validCategories.Contains(value))
                    context.AddFailure("Category", "Invalid Category, Please choose from the valid categories");
            });

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("Email is Required.")
            .EmailAddress().WithMessage("Please provide a valid Email");

        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Phone is Required.");

        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Le code postal doit être au format XX-XXX.");
    }
}


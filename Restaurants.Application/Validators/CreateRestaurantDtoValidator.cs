using FluentValidation;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Validators;
public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "Indian"];
    public CreateRestaurantDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom est obligatoire.")
            .Length(3,100);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Please provide a Description.");

        // We can also use .Must(validCategories.Contains).WithMessage("Invalid Category, Please choose from the valid categories")
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage("Category is Required.")
            .Custom((value, context) =>
            {
                var isValidCategory = validCategories.Contains(value);
                if (!isValidCategory)
                {
                    context.AddFailure("Category", "Invalid Category, Please choose from the valid categories");
                }
            });


        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("Email is Required.")
            .EmailAddress().WithMessage("Please provide a valid Email");

        RuleFor(x => x.ContactNumber)
            .NotEmpty().WithMessage("Phone is Required.");


        RuleFor(x => x.PostalCode)
            .Matches(@"^\d{2}-\d{3}$").WithMessage("Le code postal doit contenir 5 chiffres XX-XXX.");
    }

    
}

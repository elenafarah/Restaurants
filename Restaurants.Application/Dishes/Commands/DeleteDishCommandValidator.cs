using FluentValidation;

namespace Restaurants.Application.Dishes.Commands;
public sealed class DeleteDishCommandValidator : AbstractValidator<DeleteDishCommand>
{
    public DeleteDishCommandValidator()
    {
    }
}

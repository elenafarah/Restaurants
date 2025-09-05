using FluentValidation;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants.Queries;

namespace Restaurants.Application.Validators;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private static readonly int[] AllowedPageSizes = { 5, 10, 15, 30, 50 };

    private static readonly string[] AllowedSortBy =
    {
        nameof(RestaurantDto.Name),
        nameof(RestaurantDto.Description),
        nameof(RestaurantDto.Category)
    };

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber doit être ≥ 1.");

        RuleFor(x => x.PageSize)
            .Must(ps => AllowedPageSizes.Contains(ps))
            .WithMessage($"PageSize doit être dans {{ {string.Join(", ", AllowedPageSizes)} }}");

        // (optionnel) limiter la longueur du terme de recherche
        RuleFor(x => x.SearchPhrase)
            .MaximumLength(100).WithMessage("SearchPhrase est trop long (max 100).");

        RuleFor(q => q.SortBy)
            .Must(v => AllowedSortBy.Contains(v!, StringComparer.OrdinalIgnoreCase))
            .When(q => !string.IsNullOrWhiteSpace(q.SortBy))
            .WithMessage($"SortBy est optionnel ou doit être dans: {string.Join(", ", AllowedSortBy)}");
    }
}


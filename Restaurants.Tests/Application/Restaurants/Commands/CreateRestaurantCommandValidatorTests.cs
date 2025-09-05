using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands;
using Xunit;

namespace Restaurants.Tests.Application.Restaurants.Commands
{
    public class CreateRestaurantCommandValidatorTests
    {
        private readonly CreateRestaurantCommandValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var model = new CreateRestaurantCommand { Name = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var model = new CreateRestaurantCommand { Description = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Category_Is_Invalid()
        {
            var model = new CreateRestaurantCommand { Category = "French" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void Should_Have_Error_When_ContactEmail_Is_Invalid()
        {
            var model = new CreateRestaurantCommand { ContactEmail = "invalid-email" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ContactEmail);
        }

        [Fact]
        public void Should_Have_Error_When_ContactNumber_Is_Empty()
        {
            var model = new CreateRestaurantCommand { ContactNumber = "" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ContactNumber);
        }

        [Fact]
        public void Should_Have_Error_When_PostalCode_Is_Invalid()
        {
            var model = new CreateRestaurantCommand { PostalCode = "12345" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PostalCode);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Model_Is_Valid()
        {
            var model = new CreateRestaurantCommand
            {
                Name = "Test Restaurant",
                Description = "A nice place.",
                Category = "Italian",
                ContactEmail = "test@example.com",
                ContactNumber = "123456789",
                PostalCode = "12-345"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

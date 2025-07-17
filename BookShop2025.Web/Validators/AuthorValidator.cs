using BookShop2025.Service.DTOs.Author;
using FluentValidation;

namespace BookShop2025.Web.Validators
{
    public class AuthorValidator : AbstractValidator<AuthorEditDto>
    {
        public AuthorValidator()
        {
            // Rule for FirstName
            RuleFor(author => author.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(100).WithMessage("First Name cannot exceed 100 characters.");

            // Rule for LastName
            RuleFor(author => author.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name cannot exceed 100 characters.");

            // Rule for CountryId
            RuleFor(author => author.CountryId)
                .NotEmpty().WithMessage("Country ID is required.") // For int, NotEmpty checks that it's not the default value (0)
                .GreaterThan(0).WithMessage("Country ID must be a valid positive value."); // Ensures it's a positive ID
        }
    }
}

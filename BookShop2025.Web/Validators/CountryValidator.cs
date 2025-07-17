using BookShop2025.Web.ViewModels.Country;
using FluentValidation;

namespace BookShop2025.Web.Validators
{
    public class CountryValidator : AbstractValidator<CountryEditVm>
    {
        public CountryValidator()
        {
            RuleFor(c => c.CountryName).NotEmpty().WithMessage("Required")
                .MinimumLength(3).WithMessage("Must have al least {MinLength} characters")
                .MaximumLength(100).WithMessage("No more than {MaxLength} characters");
        }
    }
}

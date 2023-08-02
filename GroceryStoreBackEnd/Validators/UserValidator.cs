using FluentValidation;
using GroceryStoreBackEnd.ViewModels;
using System.Text.RegularExpressions;

namespace GroceryStoreBackEnd.Validators
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        List<string> RolesAllowed = new List<string>() { "admin", "superAdmin", "user" };
        public UserValidator()
        {
            RuleFor(user => user.UserEmail).NotNull().NotEmpty().WithMessage("User email is required.").EmailAddress().WithMessage("Email is not valid.");

            RuleFor(user => user.UserName).NotEmpty().NotNull().WithMessage("User name is required.").MinimumLength(2).WithMessage("User name must have length greater than 2");

            RuleFor(p => p.PhoneNumber)
           .NotEmpty()
           .NotNull().WithMessage("Phone Number is required.")
           .MinimumLength(10).WithMessage("PhoneNumber must not be less than 10 characters.")
           .MaximumLength(20).WithMessage("PhoneNumber must not exceed 50 characters.");
           //.Matches(new Regex(@"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}")).WithMessage("PhoneNumber not valid");

            RuleFor(user => user.UserRole).NotNull().NotEmpty().Must(role => RolesAllowed.Contains(role) == true)
                .WithMessage("'{PropertyName}' must not be one of the three roles");

        }

    }
}

using FluentValidation;
using GroceryStoreBackEnd.ViewModels;

namespace GroceryStoreBackEnd.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginViewModel>
    {
        public UserLoginValidator() {

            RuleFor(user => user.UserEmail).NotNull().NotEmpty().WithMessage("User email is required.").EmailAddress().WithMessage("Email is not valid.");
        }
    }
}

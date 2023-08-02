using FluentValidation;
using GroceryStoreBackEnd.ViewModels;

namespace GroceryStoreBackEnd.Validators
{
    public class CartValidator : AbstractValidator<CartViewModel>
    {
        public CartValidator()
        {

            RuleFor(cartItem => cartItem.UserId).NotNull().WithMessage("UserId cannot be null").NotEmpty().WithMessage("UserId should not be empty");
            RuleFor(cartItem => cartItem.ProductId).NotNull().WithMessage("ProductId cannot be null").NotEmpty().WithMessage("ProductId should not be empty");
            RuleFor(cartItem => cartItem.Quantity).NotNull().WithMessage("Quantity cannot be null").GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        }
    }
}

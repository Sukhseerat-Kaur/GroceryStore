using FluentValidation;
using GroceryStoreBackEnd.ViewModels;

namespace GroceryStoreBackEnd.Validators
{
    public class OrderValidator :AbstractValidator<OrderViewModel>
    {
        public OrderValidator() {
            RuleFor(order => order.UserId).NotNull().WithMessage("User id must not be null").NotEmpty().WithMessage("User id must not be empty");

            RuleFor(order => order.ProductId).NotNull().WithMessage("Product id must not be null").NotEmpty().WithMessage("Product id must not be empty");

            RuleFor(order => order.Quantity).NotNull().WithMessage("Quantity must not be null").GreaterThan(0).WithMessage("Order quantity should be greater than zero");

            RuleFor(order => order.BuyingPrice).NotNull().WithMessage("Price must not be null").GreaterThan(0).WithMessage("Price should be greater than zero");


        }
    }
}

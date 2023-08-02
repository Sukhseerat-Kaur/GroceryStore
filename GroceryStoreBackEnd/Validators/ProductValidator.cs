using FluentValidation;
using GroceryStoreBackEnd.ViewModels;

namespace GroceryStoreBackEnd.Validators
{
    public class ProductValidator : AbstractValidator<ProductCreationViewModel>
    {
        public ProductValidator() {

            RuleFor(product => product.ImageFile).SetValidator(new ImageValidator());

            RuleFor(product => product.ProductName).NotNull().WithMessage("Product name cannot be null").NotEmpty().WithMessage("Product name should not be empty");

            RuleFor(product => product.ProductPrice).NotNull().WithMessage("Product Price cannot be null.").NotEmpty().WithMessage("Product Price should not be empty").GreaterThan(0).WithMessage("Product price cannot be zero");


        }
    }
}

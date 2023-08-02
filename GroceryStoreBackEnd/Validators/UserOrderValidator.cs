using FluentValidation;
using GroceryStoreBackEnd.ViewModels;

namespace GroceryStoreBackEnd.Validators
{
    public class UserOrderValidator : AbstractValidator<List<OrderViewModel>>
    {
        public UserOrderValidator() {
            RuleForEach(order => order).SetValidator(new OrderValidator());
        }
    }
}

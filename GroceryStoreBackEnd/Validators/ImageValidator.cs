using FluentValidation;

namespace GroceryStoreBackEnd.Validators
{
    public class ImageValidator: AbstractValidator<IFormFile>
    {
        List<string> AllowedFileTypes = new List<string>(){"image/jpeg", "image/jpg",  "image/png"};
        public ImageValidator()
        {
            RuleFor(image => image.Length).NotNull().NotEmpty().WithMessage("Image length cannot be null");
            RuleFor(image => image.ContentType).NotNull().Must(x => AllowedFileTypes.Contains(x) == true).WithMessage("File type is not allowed");
        }
    }
}

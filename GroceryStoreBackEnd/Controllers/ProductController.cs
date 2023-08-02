using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreBackEnd.Utilities.Mapper;
using GroceryStoreCore.DTOs;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreCore.Filters;
using System.Data;
using FluentValidation;
using FluentValidation.Results;

namespace GroceryStoreBackEnd.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(GroceryStoreExceptionFilterAttribute))]
    public class ProductController : ControllerBase
    {

        private readonly IWebHostEnvironment _environment;

        private readonly IFacade _facade;

        private readonly IValidator<ProductCreationViewModel> _productValidator;

        public ProductController(IWebHostEnvironment environment, IFacade facade, IValidator<ProductCreationViewModel> validator)
        {
            _environment = environment;
            _facade = facade;
            _productValidator = validator;
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin, superAdmin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductCreationViewModel product)
        {
            ValidationResult result = await _productValidator.ValidateAsync(product);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Login failed"
                });
            }
            var productDto = ProductMapper.ProductCreationViewModelToDto(product);
            var productId = await _facade.AddProduct(productDto, product.ImageFile, product.ProductCategories, _environment.ContentRootPath);
            return Ok(new
            {
                Message = "Product Added Successfully",
                productId,
            });
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProductViewModelWithImage>>> GetAllProduts()
        {
            try
            {
                var productDtos = await _facade.GetAllProducts();
                var productViewModels = ProductMapper.ProductWithCategoryDtoListToViewModelWithImageList(productDtos);
                return productViewModels;
            }
            catch (DataException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Cannot fetch data.",
                });
            }

        }

        [HttpGet("available-products")]
        public async Task<ActionResult<IEnumerable<ProductViewModelWithImage>>> GetAvailableProducts()
        {
            try
            {
                var productDtos = await _facade.GetAvailableProducts();
                var productViewModels = ProductMapper.ProductWithCategoryDtoListToViewModelWithImageList(productDtos);
                return productViewModels;
            }
            catch (DataException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Cannot fetch data.",
                });
            }
        }

        [HttpGet("image/{productId}")]
        public async Task<IActionResult> GetProductImage(int productId)
        {
            var imageDetails = await _facade.GetFileBytesAndType(productId);

            return File(imageDetails.Bytes, imageDetails.ContentType);
        }


        [HttpDelete("delete")]
        [Authorize(Roles = "admin, superAdmin")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int productId)
        {
            try
            {
                await _facade.DeleteProduct(productId);
                return Ok(new
                {
                    Message = "Product has been deleted successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Error = ex.Data,
                });
            }

        }

        [HttpPut("edit")]
        [Authorize(Roles = "admin, superAdmin")]

        public async Task<IActionResult> UpdateProduct([FromForm] ProductCreationViewModel product)
        {
            try
            {
                ProductDto productDto = ProductMapper.ProductCreationViewModelToDto(product);
                await _facade.UpdateProduct(productDto, product.ImageFile, product.ProductCategories, _environment.ContentRootPath);
                return Ok(new
                {
                    Message = "Product updated successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Cannot Update product"
                });
            }
        }


        [HttpGet("get-product")]
        public async Task<ActionResult<ProductViewModelWithImage>> GetProductByProductId([FromQuery] int productId)
        {
            try
            {
                var product = await _facade.GetProductById(productId);

                var productView = ProductMapper.ProductWithCategoryDtoToViewModelWithImage(product);

                return Ok(productView);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "Key Not found",
                });
            }
        }

        [HttpGet("available-stock")]
        public async Task<IActionResult> GetProductAvailableQuantity([FromQuery] int productId)
        {
            try
            {
                var quantity = await _facade.GetAvailableQuantity(productId);
                return Ok(quantity);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    ex.Message,
                    Error = "product not found in database"
                });
            }
        }
    }
}

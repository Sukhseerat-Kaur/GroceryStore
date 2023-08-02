using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreBackEnd.Utilities.Mapper;
using GroceryStoreCore.DTOs;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreCore.Filters;
using GroceryStoreBackEnd.Validators;
using FluentValidation.Results;
using FluentValidation;

namespace GroceryStoreBackEnd.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(GroceryStoreExceptionFilterAttribute))]

    public class CartController : ControllerBase
    {

        private readonly IFacade _facade;
        private readonly IValidator<CartViewModel> _cartValidator;

        public CartController(IFacade facade, IValidator<CartViewModel> cartValidator)
        {
            _facade = facade;
            _cartValidator = cartValidator;
        }

        [HttpPost("add-to-cart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddToCart([FromBody] CartViewModel cartViewModel)
        {
            ValidationResult result = await _cartValidator.ValidateAsync(cartViewModel);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Could not add item to cart"
                });
            }
            try
            {
                CartDto cartItem = CartMapper.CartViewModelToDto(cartViewModel);
                await _facade.AddToCart(cartItem);
                return Ok(new
                {
                    Message = "product added to cart."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    Message = "Product cannot be added to cart.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("get-cart")]
        [Authorize(Roles = "user")]
        public async Task<IEnumerable<CartProductViewModel>> GetCartByUserId([FromQuery] int userId)
        {
            List<CartProductDto> cartProductDtos = await _facade.GetCartByUserId(userId);
            List<CartProductViewModel> cartProducts = ProductMapper.CartProductDtoListToViewModelList(cartProductDtos);
            return cartProducts;

        }

        [HttpDelete("remove-from-cart")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int userId, [FromQuery] int productId)
        {

            await _facade.RemoveFromCart(userId, productId);
            return Ok(new
            {
                Message = "Product removed from cart"
            });
        }

        [HttpPost("update")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartViewModel cartItem)
        {
            ValidationResult result = await _cartValidator.ValidateAsync(cartItem);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Could not update cart item"
                });
            }
            try
            {
                await _facade.UpdateCartItemQuantity(cartItem.UserId, cartItem.ProductId, cartItem.Quantity);
                if (cartItem.Quantity > 0)
                {
                    return Ok(new
                    {
                        Message = "Cart has been updated successfully"
                    });
                }

                return Ok(new
                {
                    Message = "Item has been removed from cart.",
                });
            }catch(ArgumentNullException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Error="Please make sure the entered quantity doesn't exceeds the stocks available",
                });
            }
            catch(KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    ex.Message,
                    Error = "Cannot find product",
                });
            }
        }
    }
}

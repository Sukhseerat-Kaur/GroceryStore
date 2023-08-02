using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreBackEnd.ViewModels;
using GroceryStoreBackEnd.Utilities.Mapper;
using GroceryStoreCore.DTOs;
using GroceryStoreDomain.Services.Interfaces;
using GroceryStoreFacade.FacadeInterfaces;
using GroceryStoreCore.Filters;
using FluentValidation;
using GroceryStoreBackEnd.Validators;
using FluentValidation.Results;

namespace GroceryStoreBackEnd.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [ServiceFilter(typeof(GroceryStoreExceptionFilterAttribute))]
    public class OrderController : ControllerBase
    {
        private readonly IFacade _facade;
        private readonly IValidator<List<OrderViewModel>> _orderListValidator;
        public OrderController(IFacade facade, IValidator<List<OrderViewModel>> orderListValidator)
        {
            _facade = facade;
            _orderListValidator = orderListValidator;

        }

        [HttpPost("place-order")]
        [Authorize(Roles ="user")]
        public async Task<IActionResult> PlaceOrder([FromBody] List<OrderViewModel> orders)
        {
            ValidationResult result = await _orderListValidator.ValidateAsync(orders);
            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(error => new { error.ErrorCode, error.ErrorMessage }).ToList();

                return BadRequest(new
                {
                    Message = errorMessages,
                    Error = "Login failed"
                });
            }
            try
            {
                var orderDtoList = OrderMapper.OrderViewModelListToDtoList(orders);
                var orderId = await _facade.PlaceOrder(orderDtoList);
                return Ok(new
                {
                    Message = "Order placed successfully.",
                    orderId
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Order cannot be placed.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("get-orders/{userId}")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult<List<OrderProductViewModel>>> GetOrdersList(int userId)
        {

            try
            {
                var orderProductDtoList = await _facade.GetOrdersListByUserId(userId);

                var ordersViewList = OrderMapper.OrderProductDtoListToOrderProductViewList(orderProductDtoList);

                var sortedList = ordersViewList.OrderByDescending(o => o.OrderDateTime).ToList();

                return Ok(sortedList);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Unexpected error occured.",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("most-ordered")]
        [Authorize(Roles = "superAdmin, admin")]
        public async Task<ActionResult<List<ProductViewModelWithImage>>> GetMostOrderedProductsByMonthAndYear([FromQuery] int year, [FromQuery] int month, [FromQuery] int quantity)
        {
            try
            {
                var products = await _facade.GetMostOrderedProductsByMonthAndYear(year, month, quantity);

                var productViews = ProductMapper.ProductWithCategoryDtoListToViewModelWithImageList(products);
                return Ok(products);
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Cannot fetch the products",
                    Error = ex.Message,
                });
            }
        }

        [HttpGet("get-order")]
        [Authorize(Roles = "user, admin, superAdmin")]
        public async Task<ActionResult<OrderProductViewModel>> GetOrderByOrderId([FromQuery] Guid orderId)
        {
            try
            {
                var order = await _facade.GetOrderByOrderId(orderId);
                var orderView = OrderMapper.OrderDtoToOrderProductViewModel(order);
                return orderView;
            }
            catch(KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    Error= "Cannot get Order"
                });
            }
        }

    }
}
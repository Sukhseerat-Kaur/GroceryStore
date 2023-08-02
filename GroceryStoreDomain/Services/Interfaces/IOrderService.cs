using GroceryStoreCore.DTOs;

namespace GroceryStoreDomain.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> PlaceOrder(OrderDto order);

        Task<List<OrderDto>> GetOrdersByUserId(int userId);

        Task<List<OrderDto>> GetOrderByMonthAndYear(int year, int month);

        Task<List<ProductIdQuantity>> GetMostOrderedProductsByMonthAndYear(int year, int month, int quantity);

        Task<OrderDto> GetOrderByOrderId(Guid orderId);
    }
}

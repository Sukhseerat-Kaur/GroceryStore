using GroceryStoreCore.DTOs;

namespace GroceryStore.DataLayer.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Guid> PlaceOrder(OrderDto order);

        Task<List<OrderDto>> GetOrdersByUserId(int userId);

        Task<List<OrderDto>> GetOrdersByMonthAndYear(int year, int month);

        Task<OrderDto> GetOrderByOrderId(Guid orderId);
    }
}
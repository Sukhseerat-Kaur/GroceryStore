using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStoreDomain.Services.Interfaces;

namespace GroceryStoreDomain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductIdQuantity>> GetMostOrderedProductsByMonthAndYear(int year, int month, int quantity)
        {
            var validOrders = await this.GetOrderByMonthAndYear(year, month);
            var productAndQuantityList = validOrders.Select(order => new { order.ProductId, order.Quantity });

            var mostOrdered = productAndQuantityList.GroupBy(order => order.ProductId).Select(group => new
            ProductIdQuantity(
                group.Key,
                group.Sum(item => item.Quantity)
            )).OrderByDescending(item => item.Quantity).Take(quantity).ToList();

            return mostOrdered;
        }

        public async Task<List<OrderDto>> GetOrderByMonthAndYear(int year, int month)
        {
            return await _unitOfWork.OrderRepository.GetOrdersByMonthAndYear(year, month);
        }

        public async Task<List<OrderDto>> GetOrdersByUserId(int userId)
        {
            return await _unitOfWork.OrderRepository.GetOrdersByUserId(userId);
        }

        public async Task<Guid> PlaceOrder(OrderDto orderDto)
        {
            //Console.WriteLine(orderDto.OrderId + " " + orderDto.UserId + " " + orderDto.ProductId + " " + orderDto.Quantity + " " + orderDto.OrderDateTime);

            var orderId = await _unitOfWork.OrderRepository.PlaceOrder(orderDto);
            return orderId;
        }

        public async Task<OrderDto> GetOrderByOrderId(Guid orderId)
        {
            return await _unitOfWork.OrderRepository.GetOrderByOrderId(orderId);
        }
    }
}
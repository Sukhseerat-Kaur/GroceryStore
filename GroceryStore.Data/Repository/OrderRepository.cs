using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStore.DataLayer.Utilities.Mapper;
using GroceryStoreCore.DTOs;
using Microsoft.Extensions.Logging;

namespace GroceryStore.DataLayer.Repository
{


    public class OrderRepository : IOrderRepository
    {
        private readonly GroceryStoreDbContext _dbContext;
        private readonly ILogger<OrderRepository> _logger;
        public OrderRepository(GroceryStoreDbContext groceryStoreDbContext, ILogger<OrderRepository> logger)
        {
            _dbContext = groceryStoreDbContext;
            _logger = logger;
        }

        public async Task<List<OrderDto>> GetOrdersByUserId(int userId)
        {
            var ordersByUser = await _dbContext.Orders.Where(order => order.UserId == userId).ToListAsync();

            var orderEntityList = OrderMapper.OrderEntityListToDtoList(ordersByUser);
            _logger.LogInformation($"Order for User with Id {userId} is fetched from database at time {DateTime.Now}");

            return orderEntityList;
        }
        public async Task<List<OrderDto>> GetOrdersByMonthAndYear(int year, int month)
        {
            var orderEntityList = await _dbContext.Orders.Where(order => order.OrderDateTime.Year == year && order.OrderDateTime.Month == month).ToListAsync();

            var orderDtoList = OrderMapper.OrderEntityListToDtoList(orderEntityList);

            _logger.LogInformation($"Most order products by month and year is fetched at time {DateTime.Now} ");

            return orderDtoList;
        }

        public async Task<Guid> PlaceOrder(OrderDto order)
        {
            var orderEntity = OrderMapper.OrderDtoToOrderEntity(order);
            _dbContext.Orders.Add(orderEntity);

            await _dbContext.SaveChangesAsync();

            _logger.LogDebug($"Order placed by user with Id {orderEntity.UserId} for {orderEntity.ProductId} with quantity {orderEntity.Quantity} at time {DateTime.Now}");

            return orderEntity.OrderId;
        }

        public async Task<OrderDto> GetOrderByOrderId(Guid orderId)
        {
            try
            {
                var order = await _dbContext.Orders.Where(@order => @order.OrderId == orderId).FirstOrDefaultAsync();
                if(order == null)
                {
                    throw new KeyNotFoundException($"Order with Id {orderId} doesn't exist.");
                }
                var orderDto = OrderMapper.OrderEntityToDto(order);
                return orderDto;
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

    }
}
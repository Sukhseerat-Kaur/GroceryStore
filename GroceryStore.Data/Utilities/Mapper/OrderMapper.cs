using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Utilities.Mapper
{
    public class OrderMapper
    {
        public static OrderEntity OrderDtoToOrderEntity(OrderDto orderDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDto, OrderEntity>());
            var mapper = config.CreateMapper();

            OrderEntity order = mapper.Map<OrderEntity>(orderDto);
            return order;
        }

        public static OrderDto OrderEntityToDto(OrderEntity order)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderEntity, OrderDto>());
            var mapper = config.CreateMapper();

            OrderDto orderDto = mapper.Map<OrderDto>(order);
            return orderDto;
        }

        public static List<OrderDto> OrderEntityListToDtoList(List<OrderEntity> orderEntities)
        {
            List<OrderDto> orderDtos = new List<OrderDto>();

            foreach (var orderEntity in orderEntities)
            {
                orderDtos.Add(OrderMapper.OrderEntityToDto(orderEntity));
            }
            return orderDtos;
        }

    }
}
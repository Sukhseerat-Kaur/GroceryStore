using AutoMapper;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreCore.DTOs;

namespace GroceryStoreBackEnd.Utilities.Mapper
{
    public class OrderMapper
    {
        public static OrderDto OrderViewModelToDto(OrderViewModel orderViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderViewModel, OrderDto>());
            var mapper = config.CreateMapper();

            OrderDto orderDto = mapper.Map<OrderDto>(orderViewModel);
            return orderDto;
        }

        public static OrderProductViewModel OrderDtoToOrderProductViewModel(OrderDto orderDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDto, OrderProductViewModel>());
            var mapper = config.CreateMapper();
            OrderProductViewModel order = mapper.Map<OrderProductViewModel>(orderDto);
            return order;
        }

        public static List<OrderDto> OrderViewModelListToDtoList(List<OrderViewModel> orderViewModelList)
        {
            var orders = new List<OrderDto>();
            foreach(var orderViewModel in orderViewModelList) { 
                orders.Add(OrderMapper.OrderViewModelToDto(orderViewModel));
            }
            return orders;
        }

        public static OrderProductViewModel OrderProductDtoToViewModel(OrderProductDto orderProductDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderProductDto, OrderProductViewModel>());
            var mapper = config.CreateMapper();
            OrderProductViewModel order = mapper.Map<OrderProductViewModel>(orderProductDto);
            return order;
        }

        public static List<OrderProductViewModel> OrderProductDtoListToOrderProductViewList(List<OrderProductDto> orderDtoList)
        {
            var orders = new List<OrderProductViewModel>();
            foreach(var orderDto in orderDtoList)
            {
                orders.Add(OrderMapper.OrderProductDtoToViewModel(orderDto));
            }
            return orders;
        }
    }
}
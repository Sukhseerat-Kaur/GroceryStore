using AutoMapper;
using GroceryStoreCore.DTOs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreFacade.Utilities.Mapper
{
    public class OrderMapper
    {
        public static OrderProductDto OrderDtoToOrderProductDto(OrderDto orderDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<OrderDto, OrderProductDto>());
            var mapper = config.CreateMapper();

            OrderProductDto orderProductDto = mapper.Map<OrderProductDto>(orderDto);
            return orderProductDto;
        }
    }
}

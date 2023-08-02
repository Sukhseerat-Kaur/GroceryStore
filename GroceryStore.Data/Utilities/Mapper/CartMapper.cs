using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Utilities.Mapper
{
    public class CartMapper
    {
        public static CartEntity CartDtoToEntity(CartDto cartDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CartDto, CartEntity>());
            var mapper = config.CreateMapper();

            CartEntity cartItem = mapper.Map<CartEntity>(cartDto);
            return cartItem;
        }

        public static CartDto CartEntityToDto(CartEntity cartEntity)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CartEntity, CartDto>());
            var mapper = config.CreateMapper();

            CartDto cartItem = mapper.Map<CartDto>(cartEntity);
            return cartItem;
        }

        public static List<CartEntity> CartDtoListToEntityList(List<CartDto> cartDtos)
        {
            List<CartEntity> cartEntities = new List<CartEntity>();
            foreach(CartDto cartDto in cartDtos)
            {
                cartEntities.Add(CartMapper.CartDtoToEntity(cartDto));
            }
            return cartEntities;
        }

        public static List<CartDto> CartEntityListToDtoList(List<CartEntity> cartEntities)
        {
            List<CartDto> cartDtos = new List<CartDto>();

            foreach (CartEntity cartItem in cartEntities)
            {
                cartDtos.Add(CartMapper.CartEntityToDto(cartItem));
            }
            return cartDtos;
        }
    }
}

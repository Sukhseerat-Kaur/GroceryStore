using AutoMapper;

using GroceryStoreBackEnd.ViewModels;
using GroceryStoreCore.DTOs;

namespace GroceryStoreBackEnd.Utilities.Mapper
{
    public class CartMapper
    {
        public static CartDto CartViewModelToDto(CartViewModel cartViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CartViewModel, CartDto>());
            var mapper = config.CreateMapper();

            CartDto cartDto = mapper.Map<CartDto>(cartViewModel);
            return cartDto;
        }

        public static CartViewModel CartDtoToViewModel(CartDto cartDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<CartDto, CartViewModel>());
            var mapper = config.CreateMapper();

            CartViewModel cartViewModel = mapper.Map<CartViewModel>(cartDto);
            return cartViewModel;
        }


    }
}

using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStoreBackEnd.ViewModels;

namespace GroceryStoreBackEnd.Utilities.Mapper
{
    public class UserMapper
    {
        public static UserDto UserViewModelToDto(UserViewModel userViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserViewModel, UserDto>());
            var mapper = config.CreateMapper();

            UserDto userDto = mapper.Map<UserDto>(userViewModel);
            return userDto;
        }

        public static UserDto UserWithoutPasswordToUserDto(UserWithoutPasswordDto userWithoutPasswordDto, string password)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserWithoutPasswordDto, UserDto>());
            var mapper = config.CreateMapper();

            UserDto userDto = mapper.Map<UserDto>(userWithoutPasswordDto);
            userDto.Password = password;
            return userDto;
        }

        public static UserDto UserLoginViewModelToDto(UserLoginViewModel userLoginViewModel)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserLoginViewModel, UserDto>());
            var mapper = config.CreateMapper();

            UserDto userDto = mapper.Map<UserDto>(userLoginViewModel);
            userDto.Password = userLoginViewModel.Password;
            return userDto;
        }

        public static UserViewModel UserDtoToViewModel(UserDto userDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserViewModel>());
            var mapper = config.CreateMapper();

            UserViewModel user = mapper.Map<UserViewModel>(userDto);
            return user;
        }

        public static List<UserViewModel> UserDtoListToViewModelList(List<UserDto> userDtoList)
        {
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach(var userDto in userDtoList)
            {
                userViewModels.Add(UserMapper.UserDtoToViewModel(userDto));
            }
            return userViewModels;
        }
    }
}

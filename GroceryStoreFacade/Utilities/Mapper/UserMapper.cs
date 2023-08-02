using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GroceryStoreCore.DTOs;

namespace GroceryStoreFacade.Utilities.Mapper
{
    public class UserMapper
    {
        public static UserDto UserWithoutPasswordToUserDto(UserWithoutPasswordDto userWithoutPasswordDto, string password)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserWithoutPasswordDto, UserDto>());
            var mapper = config.CreateMapper();

            UserDto userDto = mapper.Map<UserDto>(userWithoutPasswordDto);
            userDto.Password = password;
            return userDto;
        }

        public static UserWithTokenDto UserWithoutPasswordToUserWithTokenDto(UserWithoutPasswordDto userWithoutPasswordDto, string token)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserWithoutPasswordDto, UserWithTokenDto>());
            var mapper = config.CreateMapper();

            UserWithTokenDto userWithTokenDto = mapper.Map<UserWithTokenDto>(userWithoutPasswordDto);
            userWithTokenDto.Token = token;
            return userWithTokenDto;
        }
    }
}

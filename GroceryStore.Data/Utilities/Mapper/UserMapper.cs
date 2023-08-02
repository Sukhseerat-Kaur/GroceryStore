using AutoMapper;

using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Utilities.Mapper
{
    public class UserMapper
    {
        public static UserEntity UserDtoToEntity(UserDto userDto)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserEntity>());
            var mapper = config.CreateMapper();

            UserEntity user = mapper.Map<UserEntity>(userDto);
            return user;
        }
        public static UserDto UserEntityToDto(UserEntity user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserDto>());
            var mapper = config.CreateMapper();

            UserDto userDto = mapper.Map<UserDto>(user);
            return userDto;
        }

        public static UserWithoutPasswordDto UserEntityToUserWithoutPasswordDto(UserEntity user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserEntity, UserWithoutPasswordDto>());
            var mapper = config.CreateMapper();

            UserWithoutPasswordDto userDto = mapper.Map<UserWithoutPasswordDto>(user);
            return userDto;
        }
        public static List<UserWithoutPasswordDto> UserEntityListToUserWithoutPasswordDtoList(List<UserEntity> userEntities)
        {
            List<UserWithoutPasswordDto> users = new List<UserWithoutPasswordDto>();
            foreach(var userEntity in userEntities)
            {
                users.Add(UserMapper.UserEntityToUserWithoutPasswordDto(userEntity));
            }
            return users;
        }

        public static List<UserDto> UserEntityListToDtoList(List<UserEntity> userEntities)
        {
            List<UserDto> users = new List<UserDto>();
            foreach (var userEntity in userEntities)
            {
                users.Add(UserMapper.UserEntityToDto(userEntity));
            }
            return users;
        }
    }
}

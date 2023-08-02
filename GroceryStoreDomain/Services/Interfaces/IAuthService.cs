using GroceryStoreCore.DTOs;

namespace GroceryStoreDomain.Services.Interfaces
{
    public interface IAuthService
    {
        string Authenticate(UserDto user);


    }
}
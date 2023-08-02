using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStoreDomain.Services.Interfaces;

namespace GroceryStoreDomain.Services
{
    public class UserServices : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddUser(UserDto user)
        {
            var userId = await _unitOfWork.UserRepository.AddUser(user);
            return userId;
        }

        public async Task<UserWithoutPasswordDto> GetUser(string userEmail, string userPassword)
        {
            UserWithoutPasswordDto user = await _unitOfWork.UserRepository.GetUser(userEmail, userPassword);
            return user;
        }

        public async Task<string> GetUserRole(int userId)
        {
            return await _unitOfWork.UserRepository.GetUserRole(userId);
        }

        public async Task<UserWithoutPasswordDto> GetUserById(int userId)
        {
            return await _unitOfWork.UserRepository.GetUserById(userId);
        }

        public async Task RemoveAdminAccess(int userId)
        {
            await _unitOfWork.UserRepository.RemoveAdminAccess(userId);
        }

        public async Task<List<UserDto>> GetAllAdmins()
        {
            return await _unitOfWork.UserRepository.GetAllAdmins();
        }
    }
}

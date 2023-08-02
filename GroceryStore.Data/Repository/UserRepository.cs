using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Entities;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Utilities.Mapper;
using GroceryStore.DataLayer.Context;
using Microsoft.Extensions.Logging;

namespace GroceryStore.DataLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GroceryStoreDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(GroceryStoreDbContext groceryStoreDbContext, ILogger<UserRepository> logger)
        {
            _dbContext = groceryStoreDbContext;
            _logger = logger;
        }

        public async Task<string> GetUserRole(int userId)
        {
            try
            {
                string role = await _dbContext.Users.Where(@user => @user.UserId == userId).Select(@user => @user.UserRole).FirstOrDefaultAsync();
                if (role == null)
                {
                    throw new KeyNotFoundException($"User with {userId} doesn't exist.");
                }
                _logger.LogInformation($"User role of {userId} is accessed at time {DateTime.Now}");
                return role;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task<UserWithoutPasswordDto> GetUser(string userEmail, string userPassword)
        {
            try
            {
                UserEntity user = await _dbContext.Users.Where(@user => @user.UserEmail == userEmail && @user.Password == userPassword).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException($"Cannot find a user with email : {userEmail} and password : {userPassword}");
                }
                UserWithoutPasswordDto userDetails = UserMapper.UserEntityToUserWithoutPasswordDto(user);
                _logger.LogInformation($"User details with {user.UserId} is accessed at time {DateTime.Now}");

                return userDetails;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task<int> AddUser(UserDto user)
        {
            var userEntity = await _dbContext.Users.FirstOrDefaultAsync(currentUser => currentUser.UserEmail == user.UserEmail);
            if (userEntity != null)
            {
                throw new InvalidOperationException("User Already Exists in the database");
            }
            userEntity = UserMapper.UserDtoToEntity(user);
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"User with id {user.UserId} is added to the database");
            return userEntity.UserId;
        }

        public async Task<UserWithoutPasswordDto> GetUserById(int userId)
        {
            try
            {
                var user = await _dbContext.Users.Where(@user => @user.UserId == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException($"Cannot find User with Id {userId}");
                }
                var userWithoutPassword = UserMapper.UserEntityToUserWithoutPasswordDto(user);
                return userWithoutPassword;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task RemoveAdminAccess(int userId)
        {
            try
            {
                var user = await _dbContext.Users.Where(@user => @user.UserId == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new KeyNotFoundException($"Cannot find User with Id {userId}");
                }
                user.UserRole = "user";
                await _dbContext.SaveChangesAsync();
                _logger.LogWarning($"Admin access for User with id {userId} is removed from the database at time {DateTime.Now}");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }


        public async Task<List<UserDto>> GetAllAdmins()
        {

           var adminUsers = await _dbContext.Users.Where(user => user.UserRole == "admin").ToListAsync();
            var adminUsersWithoutPasswords = UserMapper.UserEntityListToDtoList(adminUsers);
            return adminUsersWithoutPasswords;
        }
    }
}

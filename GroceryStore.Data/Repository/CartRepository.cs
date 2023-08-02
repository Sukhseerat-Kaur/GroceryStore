using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.Entities;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Utilities.Mapper;
using Microsoft.Extensions.Logging;

namespace GroceryStore.DataLayer.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly GroceryStoreDbContext _dbContext;
        private readonly ILogger<CartRepository> _logger;
        public CartRepository(GroceryStoreDbContext groceryStoreDbContext, ILogger<CartRepository> logger)
        {
            _dbContext = groceryStoreDbContext;
            _logger = logger;
        }

        public async Task<List<CartDto>> GetCartByUserId(int userId)
        {
            try
            {
                var cartEntityList = await _dbContext.Cart.Where(cartItem => cartItem.UserId == userId).ToListAsync<CartEntity>();
                if (cartEntityList == null)
                {
                    throw new KeyNotFoundException($"Cannot fetch cart details for the user with id {userId}");
                }
                var cartDtoList = CartMapper.CartEntityListToDtoList(cartEntityList);
                _logger.LogInformation($"Cart Items of {userId} is fetched at {DateTime.Now}");
                return cartDtoList;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }
        public async Task<bool> ExistInCart(int userId, int productId)
        {
            var cartItems = await this.GetCartByUserId(userId);
            return cartItems.Any((product) => product.ProductId == productId);
        }
        public async Task AddToCart(CartDto cartItem)
        {
            var cartItemEntity = CartMapper.CartDtoToEntity(cartItem);
            await _dbContext.Cart.AddAsync(cartItemEntity);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"Product with product Id {cartItem.ProductId} with quantity {cartItem.Quantity} is added to cart of user with user id {cartItem.UserId} at {DateTime.Now}");
        }

        public async Task RemoveFromCart(int userId, int productId)
        {
            var cartItem = await _dbContext.Cart.Where(currentCartItem => currentCartItem.ProductId == productId && currentCartItem.UserId == userId).FirstOrDefaultAsync();
            if (cartItem == null)
            {
                throw new InvalidOperationException("Product doesn't exist in cart.");
            }
            _dbContext.Cart.Remove(cartItem);

            await _dbContext.SaveChangesAsync();
            _logger.LogWarning($"Product with product Id {productId} removed from cart of user with user Id {userId}");
        }

        public async Task UpdateQuantity(int userId, int productId, int newQuantity)
        {
            CartEntity cartItem = await _dbContext.Cart.Where(currentCartItem => currentCartItem.UserId == userId && currentCartItem.ProductId == productId).FirstOrDefaultAsync();
            if (cartItem == null)
            {
                throw new InvalidOperationException("Item doesn't exist.");
            }
            if (newQuantity == 0)
            {
                await this.RemoveFromCart(userId, productId);
                return;
            }
            _logger.LogDebug($"cart item with product id {productId} is changed from quantity {cartItem.Quantity} to {newQuantity} at time {DateTime.Now}");
            cartItem.Quantity = newQuantity;
            await _dbContext.SaveChangesAsync();
        }

    }
}

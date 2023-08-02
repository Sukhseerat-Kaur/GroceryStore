using GroceryStoreCore.DTOs;

namespace GroceryStoreDomain.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCart(CartDto cartDto);

        Task RemoveFromCart(int userId, int productId);

        Task UpdateQuantity(int userId, int productId, int newQuantity);

        Task<List<CartDto>> GetCartByUserId(int userId);
    }
}

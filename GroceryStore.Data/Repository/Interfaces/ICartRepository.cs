using GroceryStoreCore.DTOs;

namespace GroceryStore.DataLayer.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task AddToCart(CartDto cartItem);

        Task RemoveFromCart(int userId, int productId);

        Task UpdateQuantity(int userId, int productId, int newQuantity);

        Task<List<CartDto>> GetCartByUserId(int userId);
        Task<bool> ExistInCart(int userId, int productId);
    }
}

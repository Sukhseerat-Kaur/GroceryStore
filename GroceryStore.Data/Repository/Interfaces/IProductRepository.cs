using GroceryStoreCore.DTOs;

namespace GroceryStore.DataLayer.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<int> AddProduct(ProductDto product);

        Task<int> UpdateProduct(ProductDto productDto);

        Task<ProductDto> GetProductById(int productId);

        Task<List<ProductDto>> GetAllProducts();

        Task DeleteProduct(int productId);

        Task UpdateQuantity(int productId, int newQuantity);

        Task<int> GetProductAvailableQuantity(int productId);
    }
}

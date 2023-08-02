using GroceryStoreCore.DTOs;

namespace GroceryStoreDomain.Services.Interfaces
{
    public interface IProductService
    {
        Task<int> AddProduct(ProductDto product);

        Task<int> UpdateProduct(ProductDto productDto);

        Task DeleteProduct(int productId);

        Task<ProductDto> GetProductById(int productId);

        Task<List<ProductDto>> GetAllProducts();

        Task UpdateQuantity(int productId, int newQuantity);

        Task<int> GetProductAvailableQuantity(int productId);
    }
}

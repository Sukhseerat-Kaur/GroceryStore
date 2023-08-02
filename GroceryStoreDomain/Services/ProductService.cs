using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStoreDomain.Services.Interfaces;

namespace GroceryStoreDomain.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddProduct(ProductDto product)
        {
            var productId = await _unitOfWork.ProductRepository.AddProduct(product);
            return productId;
        }

        public async Task DeleteProduct(int productId)
        {
            await _unitOfWork.ProductRepository.DeleteProduct(productId);
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {

            List<ProductDto> productDtos = await _unitOfWork.ProductRepository.GetAllProducts();

            return productDtos;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            return await _unitOfWork.ProductRepository.GetProductById(productId);
        }

        public async Task<int> UpdateProduct(ProductDto productDto)
        {
            int productId = await _unitOfWork.ProductRepository.UpdateProduct(productDto);
            return productId;
        }

        public async Task UpdateQuantity(int productId, int newQuantity)
        {
            await _unitOfWork.ProductRepository.UpdateQuantity(productId, newQuantity);
        }

        public async Task<int> GetProductAvailableQuantity(int productId)
        {
            return await _unitOfWork.ProductRepository.GetProductAvailableQuantity(productId);
        }
    }
}

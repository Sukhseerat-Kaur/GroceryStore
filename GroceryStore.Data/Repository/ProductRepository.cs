using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.Entities;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.Utilities.Mapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GroceryStore.DataLayer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly GroceryStoreDbContext _dbContext;

        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(GroceryStoreDbContext dbContext, ILogger<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProducts()
        {

            var products = await _dbContext.Products.ToListAsync<ProductEntity>();
            if (products == null)
            {
                throw new DataException("Cannot fetch products from database.");
            }
            var productDtoList = ProductMapper.ProductEntityListToDtoList(products);
            _logger.LogInformation($"Details of all products is fetched at {DateTime.Now}");
            return productDtoList;

        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Cannot find product with product id : {productId}");
                }
                var productDto = ProductMapper.ProductEntityToDto(product);
                _logger.LogInformation($"Product details of product with Id {productId} is fetched at time {DateTime.Now}");
                return productDto;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }

        }
        public async Task<int> GetProductAvailableQuantity(int productId)
        {
            var product = await _dbContext.Products.Where(product => product.ProductId == productId).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new KeyNotFoundException($"Cannot find product with product id : {productId}");
            }
            int availableQunatity = product.ProductQuantity;
            _logger.LogInformation($"Availbale quantity for product with Id {productId} is fetched from database at time {DateTime.Now}");
            return availableQunatity;

        }

        public async Task<int> AddProduct(ProductDto productDto)
        {
            var product = ProductMapper.ProductDtoToEntity(productDto);
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            int productId = product.ProductId;
            _logger.LogDebug($"Product with id {productDto.ProductId} is added to the database at time {DateTime.Now}");
            return productId;
        }

        public async Task DeleteProduct(int productId)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with {productId} cannot be found");
                }
                product.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
                _logger.LogWarning($"Product with id {productId} is deleted from the database at time {DateTime.Now}");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }


        public async Task<int> UpdateProduct(ProductDto productDto)
        {
            try
            {
                var product = _dbContext.Products.Find(productDto.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with {productDto.ProductId} cannot be found");
                }
                product.ProductName = productDto.ProductName;
                product.ProductQuantity = productDto.ProductQuantity;
                product.ProductPrice = productDto.ProductPrice;
                product.ProductDescription = productDto.ProductDescription;
                product.ProductDiscount = productDto.ProductDiscount;
                product.ImagePath = productDto.ImagePath;
                await _dbContext.SaveChangesAsync();
                _logger.LogDebug($"Product with id {product.ProductId} is updated at time {DateTime.Now}");
                return product.ProductId;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }

        }

        public async Task UpdateQuantity(int productId, int newQuantity)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with {productId} cannot be found");
                }
                product.ProductQuantity = newQuantity;
                await _dbContext.SaveChangesAsync();
                _logger.LogDebug($"Quantity for the product with id {product.ProductId} is changed to {newQuantity} at time {DateTime.Now}");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

    }
}

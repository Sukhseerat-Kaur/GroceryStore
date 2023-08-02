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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly GroceryStoreDbContext _dbContext;

        private readonly ILogger<ProductCategoryRepository> _logger;

        public ProductCategoryRepository(GroceryStoreDbContext dbContext, ILogger<ProductCategoryRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int[]> GetAllCategoyIdsByProductId(int productId)
        {
            try
            {
                var categoryIds = await _dbContext.ProductCategories.Where(productCategory => productCategory.ProductId == productId).Select(productCategory => productCategory.CategoryId).ToArrayAsync<int>();
                if (categoryIds == null)
                {
                    throw new DataException($"Cannot fetch the categories for Product with product id {productId}");
                }
                _logger.LogInformation($"Categories for product with product id {productId} is fetched at time {DateTime.Now}");
                return categoryIds;
            }
            catch (DataException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task AddProductCategory(ProductCategoryDto productCategory)
        {
            ProductCategoryEntity productCategoryEntity = ProductCategoryMapper.ProductCategoryDtoToEntity(productCategory);
            await _dbContext.ProductCategories.AddAsync(productCategoryEntity);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"Categories added for product with Id {productCategory.ProductId}");
        }

        public async Task DeletedCategoriesWithProductId(int productId)
        {
            var productCategories = await _dbContext.ProductCategories.Where(productCategory => productId == productCategory.ProductId).ToListAsync();
            if(productCategories.Count ==  0)
            {
                return;
            }
            _dbContext.ProductCategories.RemoveRange(productCategories);
            _logger.LogWarning($"Deleted categories of product with Id {productId}");
            await _dbContext.SaveChangesAsync();
        }

    }
}

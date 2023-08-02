using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GroceryStore.DataLayer.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly GroceryStoreDbContext _dbContext;

        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(GroceryStoreDbContext dbContext, ILogger<CategoryRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<string[]> GetAllCategories()
        {
            try
            {
                var categories = await _dbContext.Categories.Select(category => category.CategoryName).ToArrayAsync<string>();
                if (categories == null)
                {
                    throw new DataException("Categories cannot be fetched from the database");
                }
                _logger.LogInformation("Categories fetched from the database");
                return categories;
            }
            catch (DataException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task<string> GetCategoryNameFromId(int categoryId)
        {
            try
            {
                string categoryName = await _dbContext.Categories.Where(category => categoryId == category.CategoryId).Select(category => category.CategoryName).FirstOrDefaultAsync();
                if (categoryName == null)
                {
                    throw new KeyNotFoundException($"category with category Id {categoryId} is not avabilable");
                }
                _logger.LogInformation("Categories fetched from the database");
                return categoryName;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }

        public async Task<int> GetCategoryIdFromName(string categoryName)
        {
            try
            {
                var category = await _dbContext.Categories.Where(category => category.CategoryName == categoryName).FirstOrDefaultAsync();
                if(category == null)
                {
                    throw new KeyNotFoundException($"Category with name \"{categoryName}\" doesn't exist.");
                }
                _logger.LogInformation($"Category id for category name \"{categoryName}\" is fetched");
                return category.CategoryId;
            }catch(KeyNotFoundException ex)
            {
                _logger.LogError($"{ex.Message} is thrown from : {ex.Source}\n\nStackTrace: {ex.StackTrace}\n\n");
                throw;
            }
        }
    }
}

using GroceryStore.DataLayer.Entities;
using GroceryStoreDomain.Services.Interfaces;
using GroceryStoreCore.DTOs;
using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStore.DataLayer.Utilities.Mapper;

namespace GroceryStoreDomain.Services
{
    public class ProductCategoryService : IProductCategoryService
    {

        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWork _unitOfWork;
        public ProductCategoryService(ICategoryService categoryService, IUnitOfWork unitOfWork)
        {
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductCategories(int productId, string categories)
        {
            string[] categoriesArray = categories.Split(',').ToArray<string>();
            int[] categoryIds = new int[categoriesArray.Length];

            for(int categoryIndex =0;categoryIndex < categoryIds.Length; categoryIndex++)
            {
                categoryIds[categoryIndex] = await _categoryService.GetCategoryIdFromName(categoriesArray[categoryIndex]);
            }

            for (int categoryIndex = 0; categoryIndex < categoryIds.Length; categoryIndex++)
            {
                ProductCategoryDto productCategoryDto = new ProductCategoryDto(productId, categoryIds[categoryIndex]);
                await _unitOfWork.ProductCategoryRepository.AddProductCategory(productCategoryDto);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletedCategoriesWithProductId(int productId)
        {
            await _unitOfWork.ProductCategoryRepository.DeletedCategoriesWithProductId(productId);
        }

        public async Task<string[]> GetAllCategoriesByProductId(int productId)
        {
            int[] categoryIds = await _unitOfWork.ProductCategoryRepository.GetAllCategoyIdsByProductId(productId);

            string[] categories = new string[categoryIds.Length];
            for(int i=0; i<categoryIds.Length; i++)
            {
                string? categoryName = await _categoryService.GetCategoryNameFromId(categoryIds[i]);
                if(categoryName != null)
                {
                    categories[i] = categoryName;
                }
            }
            return categories;
        }
    }
}

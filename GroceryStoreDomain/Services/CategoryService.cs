using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.UnitOfWork.Interfaces;
using GroceryStoreDomain.Services.Interfaces;

namespace GroceryStoreDomain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public async Task<string[]> GetAllCategories()
        {
            return await _unitOfWork.CategoriesRepository.GetAllCategories();
        }

        public async Task<string> GetCategoryNameFromId(int categoryId)
        {
            return await _unitOfWork.CategoriesRepository.GetCategoryNameFromId(categoryId);
        }

        public async Task<int> GetCategoryIdFromName(string categoryName)
        {
            return await _unitOfWork.CategoriesRepository.GetCategoryIdFromName(categoryName);
        }
    }
}

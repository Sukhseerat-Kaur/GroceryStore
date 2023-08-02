
namespace GroceryStoreDomain.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<string[]> GetAllCategories();

        Task<string> GetCategoryNameFromId(int categoryId);

        Task<int> GetCategoryIdFromName(string categoryName);
    }
}

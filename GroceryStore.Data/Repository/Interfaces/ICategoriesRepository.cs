namespace GroceryStore.DataLayer.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<string[]> GetAllCategories();

        Task<string> GetCategoryNameFromId(int categoryId);

        Task<int> GetCategoryIdFromName(string categoryName);
    }
}

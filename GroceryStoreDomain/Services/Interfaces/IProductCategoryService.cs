namespace GroceryStoreDomain.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task AddProductCategories(int productId, string categories);
        Task<string[]> GetAllCategoriesByProductId(int productId);

        Task DeletedCategoriesWithProductId(int productId);
    }
}

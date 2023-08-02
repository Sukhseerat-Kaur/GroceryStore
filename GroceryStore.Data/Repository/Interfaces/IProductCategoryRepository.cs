using GroceryStoreCore.DTOs;

namespace GroceryStore.DataLayer.Repository.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task AddProductCategory(ProductCategoryDto productCategory);
        
        Task<int[]> GetAllCategoyIdsByProductId(int productId);

        Task DeletedCategoriesWithProductId(int productId);
    }
}

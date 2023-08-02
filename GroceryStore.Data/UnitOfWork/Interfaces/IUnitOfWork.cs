using GroceryStore.DataLayer.Repository.Interfaces;

namespace GroceryStore.DataLayer.UnitOfWork.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IUserRepository UserRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        ICategoryRepository CategoriesRepository { get; }
        IReviewRepository ReviewRepository { get; }


        Task SaveAsync();

        Task CreateTransaction();

        Task Commit();

        Task Rollback();
    }
}

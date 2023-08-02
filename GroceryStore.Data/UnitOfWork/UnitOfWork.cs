using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

using GroceryStore.DataLayer.Context;
using GroceryStore.DataLayer.Repository.Interfaces;
using GroceryStore.DataLayer.UnitOfWork.Interfaces;

namespace GroceryStore.DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GroceryStoreDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryRepository _categoriesRepository;
        private readonly IReviewRepository _reviewRepository;

        private IDbContextTransaction transaction;

        public UnitOfWork(GroceryStoreDbContext dbContext, IProductRepository productRepository, IUserRepository userRepository, ICartRepository cartRepository, IOrderRepository orderRepository, IProductCategoryRepository productCategoryRepository, ICategoryRepository categoriesRepository, IReviewRepository reviewRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _productCategoryRepository = productCategoryRepository;
            _categoriesRepository = categoriesRepository;
            _reviewRepository = reviewRepository;
        }

        public IProductRepository ProductRepository => _productRepository;
        public IUserRepository UserRepository => _userRepository;
        public ICartRepository CartRepository => _cartRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IProductCategoryRepository ProductCategoryRepository => _productCategoryRepository;
        public ICategoryRepository CategoriesRepository => _categoriesRepository;

        public IReviewRepository ReviewRepository => _reviewRepository;

        public async Task SaveAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CreateTransaction()
        {
            this.transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await this.transaction.CommitAsync();
        }

        public async Task Rollback()
        {
            await this.transaction.RollbackAsync();

            await this.transaction.DisposeAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;

using GroceryStore.DataLayer.Entities;
namespace GroceryStore.DataLayer.Context
{
    public class GroceryStoreDbContext : DbContext
    {
        public GroceryStoreDbContext(DbContextOptions<GroceryStoreDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductCategoryEntity>().HasKey(c => new { c.ProductId, c.CategoryId });

            modelBuilder.Entity<OrderEntity>().HasKey(c => new { c.OrderId, c.ProductId, c.UserId });

            modelBuilder.Entity<CartEntity>().HasKey(c => new { c.UserId, c.ProductId });

            modelBuilder.Entity<OrderEntity>().Property(e => e.OrderDateTime).HasColumnType("datetime2");

            modelBuilder.Entity<ReviewEntity>().HasKey(e => new { e.ProductId, e.UserId, e.Time });
        }
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }

        public DbSet<CartEntity> Cart{ get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        public DbSet<ReviewEntity> Reviews { get; set; }
    }
}

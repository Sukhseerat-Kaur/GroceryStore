using GroceryStore.DataLayer.Entities;

namespace GroceryStore.DataLayer.Context
{
    public class DbSeeder
    {
        private readonly GroceryStoreDbContext _dbContext;

        public DbSeeder(GroceryStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedSuperAdminUser()
        {
            var superAdminUser = _dbContext.Users.FirstOrDefault(user => user.UserEmail == "admin@store.com");
            if(superAdminUser == null) {
                superAdminUser = new UserEntity
                {
                    UserName = "admin",
                    UserEmail =  "admin@store.com",
                    Password = "admin",
                    PhoneNumber = "0000000000",
                    UserRole = "superAdmin"
                };
                _dbContext.Users.Add(superAdminUser);
                _dbContext.SaveChanges();
            }
        }

        public void SeedRoles()
        {
            string[] roles = new string[] { "user", "admin", "superAdmin" };

            for(int roleIndex=0; roleIndex < roles.Length; roleIndex++)
            {
                var role = _dbContext.Roles.FirstOrDefault(role => role.RoleName == roles[roleIndex]);

                if(role == null)
                {
                    role = new RoleEntity
                    {
                        RoleName = roles[roleIndex],
                    };
                    _dbContext.Roles.Add(role);
                }
            }
            _dbContext.SaveChanges();
        }

        public void SeedCategories()
        {
            string[] categories = new string[] { "Fruits", "Vegetables", "Dairy", "Meat", "Sea Food", "Bread & Bakery", "Frozen", "Beverages", "Packed", "Sauces & Condiments", "Herbs & Spices", "Snacks", };
            for (int categoryIndex = 0; categoryIndex < categories.Length; categoryIndex++)
            {
                var category = _dbContext.Categories.FirstOrDefault(c => c.CategoryName == categories[categoryIndex]);

                if (category == null)
                {
                    category = new CategoryEntity
                    {
                        CategoryName = categories[categoryIndex],
                    };
                    _dbContext.Categories.Add(category);
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
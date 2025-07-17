using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly BookShopDbContext _dbContext;

        public CategoryRepository(BookShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public void Update(Category category)
        {
            var categoryInDb = Get(filter: c => c.CategoryId == category.CategoryId,
                   tracked: true);
            if (categoryInDb != null)
            {
                categoryInDb.CategoryName = category.CategoryName;
                categoryInDb.Description = category.Description;
                categoryInDb.IsActive = category.IsActive;

            }
        }

        public bool Exist(Category category)
        {
            return category.CategoryId == 0
                ? _dbContext.Categories.Any(c => c.CategoryName == category.CategoryName)
                : _dbContext.Categories.Any(c => c.CategoryName == category.CategoryName &&
                    c.CategoryId != category.CategoryId);
        }

    }
}

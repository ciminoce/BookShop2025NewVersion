using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        bool Exist(Category category);
        void Update(Category category);

    }

}

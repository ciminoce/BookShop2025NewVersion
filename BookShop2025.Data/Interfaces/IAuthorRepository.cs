using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Interfaces
{
    public interface IAuthorRepository:IGenericRepository<Author>
    {
        bool Exist(Author author);
        void Update(Author author);

    }

}

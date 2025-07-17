using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Interfaces
{
    public interface IPublisherRepository:IGenericRepository<Publisher>
    {
        bool Exist(Publisher publisher);
        void Update(Publisher publisher);

    }
}

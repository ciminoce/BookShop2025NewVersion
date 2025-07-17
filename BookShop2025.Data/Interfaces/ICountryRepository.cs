using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Interfaces
{
    public interface ICountryRepository:IGenericRepository<Country>
    {
        bool Exist(Country category);
        void Update(Country category);

    }

}

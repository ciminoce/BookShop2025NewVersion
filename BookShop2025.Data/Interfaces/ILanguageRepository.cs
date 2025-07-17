using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Interfaces
{
    public interface ILanguageRepository : IGenericRepository<Language>
    {
        bool Exist(Language language);
        void Update(Language language);

    }

}

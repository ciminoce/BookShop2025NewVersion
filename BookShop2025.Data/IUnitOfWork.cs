using BookShop2025.Data.Interfaces;

namespace BookShop2025.Data
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get;  }
        ICountryRepository Countries { get; }
        IAuthorRepository Authors { get; }
        IPublisherRepository Publishers { get; }
        ILanguageRepository Languages { get; }
        int Complete();
    }
}

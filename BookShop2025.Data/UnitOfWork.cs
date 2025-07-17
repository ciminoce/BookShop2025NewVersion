using BookShop2025.Data.Interfaces;
using BookShop2025.Data.Repositories;

namespace BookShop2025.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookShopDbContext _dbContext;
        private ICategoryRepository _categories;
        private ICountryRepository _countries;
        private IAuthorRepository _authors;
        private IPublisherRepository _publishers;
        private ILanguageRepository _languages;
        public UnitOfWork(BookShopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICategoryRepository Categories 
        {
            get {

                _categories ??= new CategoryRepository(_dbContext);
                return _categories;
            }
        
        
        }
        public ICountryRepository Countries 
        {
            get {

                _countries ??= new CountryRepository(_dbContext);
                return _countries;
            }
        
        
        }
        public IAuthorRepository Authors
        {
            get
            {
                _authors ??= new AuthorRepository(_dbContext);
                return _authors;
            }
        }
        public IPublisherRepository Publishers
        {
            get
            {
                _publishers ??= new PublisherRepository(_dbContext);
                return _publishers;
            }
        }
        public ILanguageRepository Languages
        {
            get
            {
                _languages ??= new LanguageRepository(_dbContext);
                return _languages;
            }
        }
        public int Complete()
        {
            return _dbContext.SaveChanges();
        }
    }
}

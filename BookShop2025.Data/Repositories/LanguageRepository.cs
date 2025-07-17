using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Repositories
{
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        private readonly BookShopDbContext _dbContext;

        public LanguageRepository(BookShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public void Update(Language language)
        {
            var languageInDb = Get(filter: c => c.LanguageId == language.LanguageId,
                   tracked: true);
            if (languageInDb != null)
            {
                languageInDb.LanguageName = language.LanguageName;

            }
        }

        public bool Exist(Language language)
        {
            return language.LanguageId == 0
                ? _dbContext.Languages.Any(c => c.LanguageName == language.LanguageName)
                : _dbContext.Languages.Any(c => c.LanguageName == language.LanguageName &&
                    c.LanguageId != language.LanguageId);
        }

    }
}

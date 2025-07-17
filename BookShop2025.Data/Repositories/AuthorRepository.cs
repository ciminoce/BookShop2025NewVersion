using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly BookShopDbContext _dbContext;

        public AuthorRepository(BookShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Author author)
        {
            var authorInDb = Get(filter: a => a.AuthorId == author.AuthorId, tracked: true);
            if (authorInDb != null)
            {
                authorInDb.FirstName = author.FirstName;
                authorInDb.LastName = author.LastName;
                authorInDb.CountryId = author.CountryId;
                //authorInDb.Country = null;

            }
        }

        public bool Exist(Author author)
        {
            return author.AuthorId == 0
                ? _dbContext.Authors.Any(a => a.FirstName == author.FirstName &&
                        a.LastName == author.LastName &&
                        a.CountryId == author.CountryId)
                : _dbContext.Authors.Any(a => a.FirstName == author.FirstName &&
                        a.LastName == author.LastName &&
                        a.CountryId == author.CountryId &&
                        a.AuthorId != author.AuthorId);
        }

    }
}

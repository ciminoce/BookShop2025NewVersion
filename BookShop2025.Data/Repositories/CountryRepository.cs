using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShop2025.Data.Repositories
{
    public class CountryRepository :GenericRepository<Country>, ICountryRepository
    {
        private readonly BookShopDbContext _dbContext;

        public CountryRepository(BookShopDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }


        public void Update(Country country)
        {
            var countryInDb=Get(filter:c=>c.CountryId==country.CountryId, tracked:true);
            if (countryInDb != null)
            {
                countryInDb.CountryName = country.CountryName;

            }
        }

        public bool Exist(Country country)
        {
            return country.CountryId == 0
                ? _dbContext.Countries.Any(c => c.CountryName == country.CountryName)
                : _dbContext.Countries.Any(c => c.CountryName == country.CountryName &&
                    c.CountryId != country.CountryId);
        }

    }
}

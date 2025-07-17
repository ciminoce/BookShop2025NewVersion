using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;

namespace BookShop2025.Data.Repositories
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        private readonly BookShopDbContext _dbContext;

        public PublisherRepository(BookShopDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Publisher publisher)
        {
            var publisherInDb = Get(filter: p => p.PublisherId == publisher.PublisherId, tracked: true);
            if (publisherInDb != null)
            {
                publisherInDb.Name = publisher.Name;
                publisherInDb.CountryId = publisher.CountryId;
                //publisherInDb.Country = null;

            }
        }

        public bool Exist(Publisher publisher)
        {
            return publisher.PublisherId == 0
                ? _dbContext.Publishers.Any(a => a.Name == publisher.Name &&
                        a.CountryId == publisher.CountryId)
                : _dbContext.Publishers.Any(a => a.Name == publisher.Name &&
                        a.CountryId == publisher.CountryId &&
                        a.PublisherId != publisher.PublisherId);
        }

    }
}

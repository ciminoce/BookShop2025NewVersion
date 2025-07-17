using BookShop2025.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookShop2025.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected BookShopDbContext _dbContext;
        protected DbSet<T> _dbSet;

        public GenericRepository(BookShopDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T? Get(Expression<Func<T, bool>>? filter = null, string? propertiesNames = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            if (propertiesNames is not null)
            {
                foreach (var propertyInclude in propertiesNames
                    .Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propertyInclude);
                }
            }
            if (tracked)
            {
                return query.FirstOrDefault();
            }
            return query.AsNoTracking().FirstOrDefault();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, 
            string? propertiesNames = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if(filter is not null)
            {
                query = query.Where(filter);
            }
            if(propertiesNames is not null)
            {
                foreach (var propertyInclude in propertiesNames
                    .Split(",",StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(propertyInclude);
                }
            }
            if (orderBy is not null)
            {
                query = orderBy(query);
            }
            return query.AsNoTracking();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}

using System.Linq.Expressions;

namespace BookShop2025.Data.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(
                Expression<Func<T, bool>>? filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                string? propertiesNames = null);
        T? Get(
            Expression<Func<T, bool>>? filter = null,
            string? propertiesNames = null,
            bool tracked = true);
        void Add(T entity);
        void Remove(T entity);
    }
}

using BookShop2025.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookShop2025.Data
{
    public class BookShopDbContext : DbContext
    {
        public BookShopDbContext(DbContextOptions options) : base(options)
        {
        }

        public BookShopDbContext()
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Language> Languages { get; set; }
    }
}

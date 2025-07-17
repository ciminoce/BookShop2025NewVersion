using BookShop2025.Data;
using BookShop2025.Data.Interfaces;
using BookShop2025.Service.Intefaces;
using BookShop2025.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop2025.Ioc
{
    public static class DI
    {
        public static IServiceProvider Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BookShopDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyConnection"));
            });
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<ILanguageService, LanguageService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services.BuildServiceProvider();
        }
    }
}

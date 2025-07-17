using BookShop2025.Service.DTOs.Country;

namespace BookShop2025.Data.Interfaces
{
    public interface ICountryService
    {
        IQueryable<CountryListDto> GetAll();
        CountryEditDto GetById(int id);
        bool Save(CountryEditDto countryDto, out List<string> errors);
        bool Remove(int id, out List<string> errors);
    }

}

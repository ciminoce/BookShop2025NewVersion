using AutoMapper;
using BookShop2025.Data;
using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;
using BookShop2025.Service.DTOs.Country;

namespace BookShop2025.Service.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool Remove(int id, out List<string> errors)
        {
            errors = new List<string>();
            Country? country = _unitOfWork.Countries.Get(filter:c=>c.CountryId==id,tracked:true);
            if (country == null)
            {
                errors.Add("Countries does not exist!!!");
            }
            _unitOfWork.Countries.Remove(country!);
            var rowsAffected = _unitOfWork.Complete();
            return rowsAffected > 0;
        }
        public IQueryable<CountryListDto> GetAll()
        {
            Func<IQueryable<Country>, IOrderedQueryable<Country>>? orderCountry = c => c.OrderBy(ca => ca.CountryName);
            var categories = _unitOfWork.Countries.GetAll(orderBy:orderCountry);
            return _mapper.ProjectTo<CountryListDto>(categories);
        }

        public CountryEditDto GetById(int id)
        {
            var country = _unitOfWork.Countries.Get(filter: c => c.CountryId == id);
            return _mapper.Map<CountryEditDto>(country);
        }

        public bool Save(CountryEditDto countryDto, out List<string> errors)
        {
            errors = new List<string>();
            Country country = _mapper.Map<Country>(countryDto);
            int rowsAffected;

            if (country.CountryId == 0) 
            {
                if (!_unitOfWork.Countries.Exist(country)) 
                {
                    _unitOfWork.Countries.Add(country);
                    rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;
                }
                else
                {
                    errors.Add("Country with this name already exists."); 
                    return false;
                }
            }
            else
            {


                if (!_unitOfWork.Countries.Exist(country)) 
                {
                    _unitOfWork.Countries.Update(country);
                    rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;
                }
                else
                {
                    errors.Add("Country with this name already exists."); 
                    return false;
                }
            }
        }
    }
}

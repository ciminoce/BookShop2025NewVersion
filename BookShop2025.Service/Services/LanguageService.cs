using AutoMapper;
using BookShop2025.Data;
using BookShop2025.Entities.Entities;
using BookShop2025.Service.DTOs.Language;
using BookShop2025.Service.Intefaces;
using System.Linq.Expressions;

namespace BookShop2025.Service.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LanguageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IQueryable<LanguageListDto> GetAll()
        {
            Expression<Func<Language, bool>>? filterLanguage = null;
            var categories = _unitOfWork.Languages.GetAll(filter: filterLanguage);
            return _mapper.ProjectTo<LanguageListDto>(categories);
        }

        public LanguageEditDto? GetById(int id)
        {
            var language = _unitOfWork.Languages.Get(filter: c => c.LanguageId == id, tracked: true);
            if (language is null) return null;
            return _mapper.Map<LanguageEditDto>(language);
        }

        public LanguageEditDto? GetByName(string name)
        {
            var language = _unitOfWork.Languages.Get(filter: c => c.LanguageName == name, tracked: true);
            if (language is null) return null;
            return _mapper.Map<LanguageEditDto>(language);
        }

        public bool Remove(int id, out List<string> errors)
        {
            errors = new List<string>();
            var language = _unitOfWork.Languages.Get(filter: c => c.LanguageId == id, tracked: true);
            if (language is null)
            {
                errors.Add("Language does not exist");
                return false;
            }
            _unitOfWork.Languages.Remove(language);
            var rowsAffected = _unitOfWork.Complete();
            return rowsAffected > 0;

        }

        public bool Save(LanguageEditDto languageDto, out List<string> errors)
        {
            errors = new List<string>();
            Language language = _mapper.Map<Language>(languageDto);
            if (language.LanguageId == 0)
            {
                if (!_unitOfWork.Languages.Exist(language))
                {
                    _unitOfWork.Languages.Add(language);
                    int rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;

                }
                else
                {
                    errors.Add("Language Already Exist!!");
                    return false;
                }

            }
            else
            {
                if (!_unitOfWork.Languages.Exist(language))
                {
                    _unitOfWork.Languages.Update(language);
                    int rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;

                }
                else
                {
                    errors.Add("Language Already Exist!!");
                    return false;
                }

            }
        }

    }
}
using BookShop2025.Service.DTOs.Language;

namespace BookShop2025.Service.Intefaces
{
    public interface ILanguageService
    {
        IQueryable<LanguageListDto> GetAll();
        bool Save(LanguageEditDto categoryDto, out List<string> errors);
        LanguageEditDto? GetById(int id);
        LanguageEditDto? GetByName(string name);
        bool Remove(int id, out List<string> errors);

    }
}

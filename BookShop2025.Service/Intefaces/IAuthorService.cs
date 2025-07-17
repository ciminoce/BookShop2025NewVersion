using BookShop2025.Service.DTOs.Author;

namespace BookShop2025.Data.Interfaces
{
    public interface IAuthorService
    {
        IQueryable<AuthorListDto> GetAll();
        bool Save(AuthorEditDto authorDto, out List<string> errors);
        AuthorEditDto? GetById(int id, bool tracked=false);
        bool Remove(int id, out List<string> errors);
    }

}

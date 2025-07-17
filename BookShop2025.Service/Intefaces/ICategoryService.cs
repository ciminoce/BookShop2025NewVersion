using BookShop2025.Service.DTOs.Category;

namespace BookShop2025.Data.Interfaces
{
    public interface ICategoryService
    {
        IQueryable<CategoryListDto> GetAll(string? statusFilter);
        bool Save(CategoryEditDto categoryDto, out List<string> errors);
        CategoryEditDto? GetById(int id);
        CategoryEditDto? GetByName(string name);
        bool Remove(int id, out List<string> errors);
    }

}

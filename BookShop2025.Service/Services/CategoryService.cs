using AutoMapper;
using BookShop2025.Data;
using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;
using BookShop2025.Service.DTOs.Category;
using System.Linq.Expressions;

namespace BookShop2025.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IQueryable<CategoryListDto> GetAll(string? statusFilter)
        {
            Expression<Func<Category, bool>>? filterCategory=null;
            switch (statusFilter)
            {
                case null:
                case "All":
                    filterCategory = null;
                    break;
                case "Active":
                    filterCategory = c => c.IsActive;
                    break;
                case "Inactive":
                    filterCategory = c => !c.IsActive;
                    break;

                default:
                    break;
            }
            var categories= _unitOfWork.Categories.GetAll(filter:filterCategory);
            return _mapper.ProjectTo<CategoryListDto>(categories);
        }

        public CategoryEditDto? GetById(int id)
        {
            var category = _unitOfWork.Categories.Get(filter:c=>c.CategoryId==id, tracked:true);
            if (category is null) return null;
            return _mapper.Map<CategoryEditDto>(category);
        }

        public CategoryEditDto? GetByName(string name)
        {
            var category = _unitOfWork.Categories.Get(filter: c => c.CategoryName == name, tracked: true);
            if (category is null) return null;
            return _mapper.Map<CategoryEditDto>(category);
        }

        public bool Remove(int id, out List<string> errors)
        {
            errors = new List<string>();
            var category = _unitOfWork.Categories.Get(filter: c => c.CategoryId == id, tracked: true);
            if (category is null)
            {
                errors.Add("Category does not exist");
                return false;
            }
            _unitOfWork.Categories.Remove(category);
            var rowsAffected = _unitOfWork.Complete();
            return rowsAffected > 0;

        }

        public bool Save(CategoryEditDto categoryDto, out List<string> errors)
        {
            errors = new List<string>();
            Category category = _mapper.Map<Category>(categoryDto);
            if (category.CategoryId==0)
            {
                if (!_unitOfWork.Categories.Exist(category))
                {
                    _unitOfWork.Categories.Add(category);
                    int rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;

                }
                else
                {
                    errors.Add("Category Already Exist!!");
                    return false;
                }

            }
            else
            {
                if (!_unitOfWork.Categories.Exist(category))
                {
                    _unitOfWork.Categories.Update(category);
                    int rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;

                }
                else
                {
                    errors.Add("Category Already Exist!!");
                    return false;
                }

            }
        }
    }
}

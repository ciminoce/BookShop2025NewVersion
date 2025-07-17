using AutoMapper;
using BookShop2025.Data.Interfaces;
using BookShop2025.Service.DTOs.Category;
using BookShop2025.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace BookShop2025.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page, int? pageSize, string? statusFilter)
        {
            int pageNumber = page ?? 1;
            int registerPerPage = pageSize ?? 5;

            var categoriesDto = _categoryService.GetAll(statusFilter);
            var pagedListDto = categoriesDto.ToPagedList(pageNumber, registerPerPage);
            var viewModelList = _mapper.Map<List<CategoryListVm>>(pagedListDto); // Mapea en memoria

            var viewModelPagedList = new StaticPagedList<CategoryListVm>(
                viewModelList,
                pagedListDto.PageNumber,
                pagedListDto.PageSize,
                pagedListDto.TotalItemCount
            );
            ViewBag.CurrentStatusFilter=statusFilter;
            return View(viewModelPagedList);
        }
        public IActionResult Upsert(int? id)
        {
            if(id is null || id == 0)
            {
                var model = new CategoryEditVm()
                {
                    CategoryId = 0
                };
                return View(model);
            }
            try
            {
                CategoryEditDto? categoryDto = _categoryService.GetById(id.Value);
                if(categoryDto is null)
                {
                    return NotFound($"Category With Id {id} Not Found!!");
                }
                CategoryEditVm categoryVm = _mapper.Map<CategoryEditVm>(categoryDto);
                return View(categoryVm);
            }
            catch (Exception)
            {

                TempData["error"]="Error while trying to get a category";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CategoryEditVm categoryVm)
        {
            if (ModelState.IsValid)
            {
                CategoryEditDto categoryDto = _mapper.Map<CategoryEditDto>(categoryVm);
                try
                {
                    if (_categoryService.Save(categoryDto, out var errors))
                    {
                        TempData["success"] = "Register Successfully Updated";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, errors.First());
                    }
                    return View(categoryVm);
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, "F!ck!! Something Happen" + ex.Message);
                }

            }
            return View(categoryVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id is null || id == 0)
            {
                return NotFound();
            }
            try
            {
                CategoryEditDto? categoryDto = _categoryService.GetById(id.Value);
                if (categoryDto is null)
                {
                    return NotFound($"Category With Id {id} Not Found!!");
                }
                if(_categoryService.Remove(id.Value, out var errors))
                {
                    TempData["success"] = "Category Succesfully Removed";
                    return RedirectToAction("Index");
                }
                else
                {
                    CategoryEditVm categoryVm = _mapper.Map<CategoryEditVm>(categoryDto);
                    ModelState.AddModelError(string.Empty, errors.First());
                    return View(categoryVm);

                }
            }
            catch (Exception)
            {

                TempData["error"] = "Error while trying to get a category";
                return RedirectToAction("Index");
            }

        }

    }
}

using AutoMapper;
using BookShop2025.Data.Interfaces;
using BookShop2025.Service.DTOs.Author;
using BookShop2025.Web.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using X.PagedList.Extensions;

namespace BookShop2025.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        public AuthorsController(IAuthorService authorService, IMapper mapper, ICountryService countryService)
        {
            _authorService = authorService;
            _mapper = mapper;
            _countryService = countryService;
        }

        public IActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int registerPerPage = pageSize ?? 5;
            var authorsDto = _authorService.GetAll();
            var pagedListDto = authorsDto.ToPagedList(pageNumber, registerPerPage);
            var viewModelList = _mapper.Map<List<AuthorListVm>>(pagedListDto); // Mapea en memoria

            var viewModelPagedList = new StaticPagedList<AuthorListVm>(
                viewModelList,
                pagedListDto.PageNumber,
                pagedListDto.PageSize,
                pagedListDto.TotalItemCount
            );
            return View(viewModelPagedList);
        }
        public IActionResult Upsert(int? id)
        {
            if (id is null || id == 0)
            {
                var viewModel = new AuthorEditVm()
                {
                    Countries = GetCountries()
                };
                return View(viewModel);
            }
            try
            {
                AuthorEditDto? authorDto = _authorService.GetById(id.Value);
                if (authorDto is null)
                {
                    return NotFound($"Author With Id {id} Not Found!!");
                }
                AuthorEditVm authorVm = _mapper.Map<AuthorEditVm>(authorDto);
                authorVm.Countries = GetCountries();
                return View(authorVm);
            }
            catch (Exception)
            {

                TempData["error"] = "Error while trying to get a author";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AuthorEditVm authorVm)
        {
            if (ModelState.IsValid)
            {
                AuthorEditDto authorDto = _mapper.Map<AuthorEditDto>(authorVm);
                try
                {
                    if (_authorService.Save(authorDto, out var errors))
                    {
                        TempData["success"] = "Register Successfully Updated";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, errors.First());
                    }
                    authorVm.Countries = GetCountries();
                    return View(authorVm);
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, "F!ck!! Something Happen" + ex.Message);
                }

            }
            authorVm.Countries = GetCountries();
            return View(authorVm);
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
                AuthorEditDto? authorDto = _authorService.GetById(id.Value);
                if (authorDto is null)
                {
                    return NotFound($"Author With Id {id} Not Found!!");
                }
                if (_authorService.Remove(id.Value, out var errors))
                {
                    TempData["success"] = "Author Succesfully Removed";
                    return RedirectToAction("Index");
                }
                else
                {
                    AuthorListVm authorVm = _mapper.Map<AuthorListVm>(authorDto);
                    var paisDto = _countryService.GetById(authorDto.CountryId);
                    authorVm.CountryName = paisDto!.CountryName;

                    ModelState.AddModelError(string.Empty, errors.First());
                    return View(authorVm);

                }
            }
            catch (Exception)
            {

                TempData["error"] = "Error while trying to get a author";
                return RedirectToAction("Index");
            }

        }

        private List<SelectListItem> GetCountries()
        {
            return _countryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.CountryName
                }).ToList();

        }
    }
}

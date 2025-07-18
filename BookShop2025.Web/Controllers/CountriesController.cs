using AutoMapper;
using BookShop2025.Data.Interfaces;
using BookShop2025.Service.DTOs.Country;
using BookShop2025.Web.ViewModels.Country;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using X.PagedList.Extensions;

namespace BookShop2025.Web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly ICountryService _countryService;
        private readonly IMapper _mapper;
        public CountriesController(ICountryService countryService, IMapper mapper)
        {
            _countryService = countryService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int registerPerPage = pageSize ?? 5;

            var categoriesDto = _countryService.GetAll();
            var pagedListDto = categoriesDto.ToPagedList(pageNumber, registerPerPage);
            var viewModelList = _mapper.Map<List<CountryListVm>>(pagedListDto); // Mapea en memoria

            var viewModelPagedList = new StaticPagedList<CountryListVm>(
                viewModelList,
                pagedListDto.PageNumber,
                pagedListDto.PageSize,
                pagedListDto.TotalItemCount
            );

            return View(viewModelPagedList);
        }
        public IActionResult Upsert(int? id)
        {
            if(id is null || id == 0)
            {
                return View(new CountryEditVm());
            }
            try
            {
                CountryEditDto? countryDto = _countryService.GetById(id.Value);
                if(countryDto is null)
                {
                    return NotFound($"Country With Id {id} Not Found!!");
                }
                CountryEditVm countryVm = _mapper.Map<CountryEditVm>(countryDto);
                return View(countryVm);
            }
            catch (Exception)
            {

                TempData["error"]="Error while trying to get a country";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CountryEditVm countryVm)
        {
            if (ModelState.IsValid)
            {
                CountryEditDto countryDto = _mapper.Map<CountryEditDto>(countryVm);
                try
                {
                    if (_countryService.Save(countryDto, out var errors))
                    {
                        TempData["success"] = "Register Successfully Updated";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, errors.First());
                    }
                    return View(countryVm);
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, "F!ck!! Something Happen" + ex.Message);
                }

            }
            return View(countryVm);
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
                CountryEditDto? countryDto = _countryService.GetById(id.Value);
                if (countryDto is null)
                {
                    return NotFound($"Country With Id {id} Not Found!!");
                }
                if(_countryService.Remove(id.Value, out var errors))
                {
                    TempData["success"] = "Country Succesfully Removed";
                    return RedirectToAction("Index");
                }
                else
                {
                    CountryEditVm countryVm = _mapper.Map<CountryEditVm>(countryDto);
                    ModelState.AddModelError(string.Empty, errors.First());
                    return View(countryVm);

                }
            }
            catch (Exception)
            {

                TempData["error"] = "Error while trying to get a country";
                return RedirectToAction("Index");
            }

        }

    }
}

using AutoMapper;
using BookShop2025.Service.Intefaces;
using BookShop2025.Web.ViewModels.Category;
using BookShop2025.Web.ViewModels.Language;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Runtime.CompilerServices;
using X.PagedList;
using X.PagedList.Extensions;

namespace BookShop2025.Web.Controllers
{
    public class LanguagesController : Controller
    {
        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;

        public LanguagesController(ILanguageService languageService,
            IMapper mapper)
        {
            _languageService = languageService;
            _mapper = mapper;
        }

        public IActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int registerPerPage = pageSize ?? 5;
            
            var languagesDto = _languageService.GetAll();
            var pagedListDto = languagesDto.ToPagedList(pageNumber, registerPerPage);
            var viewModelList = _mapper.Map<List<LanguageListVm>>(pagedListDto); // Mapea en memoria

            var viewModelPagedList = new StaticPagedList<LanguageListVm>(
                viewModelList,
                pagedListDto.PageNumber,
                pagedListDto.PageSize,
                pagedListDto.TotalItemCount
            );
            return View(viewModelPagedList);
        }
    }
}

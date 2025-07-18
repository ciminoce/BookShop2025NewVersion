using AutoMapper;
using BookShop2025.Service.DTOs.Author;
using BookShop2025.Service.DTOs.Category;
using BookShop2025.Service.DTOs.Country;
using BookShop2025.Service.DTOs.Language;
using BookShop2025.Service.DTOs.Publisher;
using BookShop2025.Web.ViewModels.Author;
using BookShop2025.Web.ViewModels.Category;
using BookShop2025.Web.ViewModels.Country;
using BookShop2025.Web.ViewModels.Language;
using BookShop2025.Web.ViewModels.Publisher;

namespace BookShop2025.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            LoadCategoryMapping();
            LoadCountryMapping();
            LoadAuthorMapping();
            LoadPublisherMapping();
            LoadLanguageMapping();
        }

        private void LoadLanguageMapping()
        {
            CreateMap<LanguageListDto, LanguageListVm>();
            CreateMap<LanguageEditVm, LanguageEditDto>().ReverseMap();
        }

        private void LoadPublisherMapping()
        {
            CreateMap<PublisherListDto, PublisherListVm>();
            CreateMap<PublisherEditDto, PublisherEditVm>().ReverseMap();
            CreateMap<PublisherEditDto, PublisherListVm>();
                
        }

        private void LoadAuthorMapping()
        {
            CreateMap<AuthorListDto, AuthorListVm>();
            CreateMap<AuthorEditDto, AuthorEditVm>().ReverseMap();
            CreateMap<AuthorEditDto, AuthorListVm>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName}, {src.FirstName}"));

        }
        private void LoadCountryMapping()
        {
            CreateMap<CountryListDto, CountryListVm>();
            CreateMap<CountryEditVm, CountryEditDto>().ReverseMap();
        }

        private void LoadCategoryMapping()
        {
            CreateMap<CategoryListDto, CategoryListVm>();
            CreateMap<CategoryEditVm, CategoryEditDto>().ReverseMap();
        }
    }
}

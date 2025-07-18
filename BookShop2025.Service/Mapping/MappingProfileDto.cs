using AutoMapper;
using BookShop2025.Entities.Entities;
using BookShop2025.Service.DTOs.Author;
using BookShop2025.Service.DTOs.Category;
using BookShop2025.Service.DTOs.Country;
using BookShop2025.Service.DTOs.Language;
using BookShop2025.Service.DTOs.Publisher;

namespace BookShop2025.Service.Mapping
{
    public class MappingProfileDto : Profile
    {
        public MappingProfileDto()
        {
            LoadCategoryMapping();
            LoadCountryMapping();
            LoadAuthorMapping();
            LoadPublisherMapping();
            LoadLanguageMapping();
        }

        private void LoadLanguageMapping()
        {
            CreateMap<Language, LanguageListDto>();
            CreateMap<Language, LanguageEditDto>().ReverseMap();
        }

        private void LoadPublisherMapping()
        {
            CreateMap<Publisher, PublisherListDto>()
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country!.CountryName));
            CreateMap<Publisher, PublisherEditDto>();
            CreateMap<PublisherEditDto, Publisher>()
                .ForMember(dest => dest.Country, opt => opt.Ignore()); // ¡Clave!

        }

        private void LoadAuthorMapping()
        {
            CreateMap<Author, AuthorListDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.LastName}, {src.FirstName}"))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Country!.CountryName));
            CreateMap<Author, AuthorEditDto>();
            /*
             * ¿Por qué ignoramos Country?
                Porque solo estás trabajando con el CountryId, 
                y no queremos que AutoMapper cree un objeto Country "nuevo", 
                que Entity Framework no conoce y puede marcar como Deleted,
                Detached, etc.
             */
            CreateMap<AuthorEditDto, Author>()
                .ForMember(dest => dest.Country, opt => opt.Ignore()); // ¡Clave!
        }

        private void LoadCountryMapping()
        {
            CreateMap<Country, CountryListDto>();
            CreateMap<Country, CountryEditDto>().ReverseMap();
        }

        private void LoadCategoryMapping()
        {
            CreateMap<Category, CategoryListDto>();
            CreateMap<Category, CategoryEditDto>().ReverseMap();
        }
    }
}

using BookShop2025.Service.DTOs.Publisher;

namespace BookShop2025.Data.Interfaces
{
    public interface IPublisherService
    {
        IQueryable<PublisherListDto> GetAll();
        bool Save(PublisherEditDto publisherDto, out List<string> errors);
        PublisherEditDto? GetById(int id, bool tracked=false);
        bool Remove(int id, out List<string> errors);
    }

}

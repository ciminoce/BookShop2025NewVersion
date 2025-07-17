using AutoMapper;
using BookShop2025.Data;
using BookShop2025.Data.Interfaces;
using BookShop2025.Entities.Entities;
using BookShop2025.Service.DTOs.Publisher;

namespace BookShop2025.Service.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublisherService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IQueryable<PublisherListDto> GetAll()
        {
            var publishers = _unitOfWork.Publishers.GetAll();
            return _mapper.ProjectTo<PublisherListDto>(publishers);
        }

        public PublisherEditDto? GetById(int id, bool tracked = false)
        {
            var publisher = _unitOfWork.Publishers.Get(filter:p=>p.PublisherId==id,tracked: true);
            if (publisher is null) return null;
            return _mapper.Map<PublisherEditDto>(publisher);
        }

        public bool Remove(int id, out List<string> errors)
        {
            errors = new List<string>();
            var publisher = _unitOfWork.Publishers.Get(filter:p=>p.PublisherId==id,tracked: true);
            if (publisher is null)
            {
                errors.Add("Publisher does not exist");
                return false;
            }
            _unitOfWork.Publishers.Remove(publisher);
            var rowsAffected = _unitOfWork.Complete();
            return rowsAffected > 0;

        }

        public bool Save(PublisherEditDto publisherDto, out List<string> errors)
        {
            errors = new List<string>();
            Publisher publisher = _mapper.Map<Publisher>(publisherDto);
            if (publisher.PublisherId == 0)
            {
                if (!_unitOfWork.Publishers.Exist(publisher))
                {
                    _unitOfWork.Publishers.Add(publisher);
                    int rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;

                }
                else
                {
                    errors.Add("Publisher Already Exist!!");
                    return false;
                }

            }
            else
            {
                if (!_unitOfWork.Publishers.Exist(publisher))
                {
                    _unitOfWork.Publishers.Update(publisher);
                    int rowsAffected = _unitOfWork.Complete();
                    return rowsAffected > 0;

                }
                else
                {
                    errors.Add("Publisher Already Exist!!");
                    return false;
                }

            }
        }

    }
}

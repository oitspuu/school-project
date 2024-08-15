using AutoMapper;
using Base;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;

namespace BLL.Services;

public class HobbyService : BaseEntityService<DAL.DTO.Hobby, BLL.DTO.Hobby, IHobbyRepository>, IHobbyService
{
    public HobbyService(IAppUnitOfWork unitOfWork, IHobbyRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<Hobby, DTO.Hobby>(mapper))
    {
        
    }
}
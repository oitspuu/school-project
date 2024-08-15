using AutoMapper;
using Base;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;

namespace BLL.Services;

public class WorkService : BaseEntityService<DAL.DTO.Work, BLL.DTO.Work, IWorkRepository>, IWorkService
{
    public WorkService(IAppUnitOfWork unitOfWork, IWorkRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<Work, DTO.Work>(mapper))
    {
        
    }
}
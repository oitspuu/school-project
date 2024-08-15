using AutoMapper;
using Base;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;

namespace BLL.Services;

public class SchoolService : BaseEntityService<DAL.DTO.School, BLL.DTO.School, ISchoolRepository>, ISchoolService
{
    public SchoolService(IAppUnitOfWork unitOfWork, ISchoolRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<School, DTO.School>(mapper))
    {
        
    }
}
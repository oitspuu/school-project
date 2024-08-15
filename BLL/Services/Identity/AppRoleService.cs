using AutoMapper;
using Base;
using BLL.Interfaces.Identity;
using DAL;
using DAL.DTO.Identity;
using DAL.Interfaces.Identity;

namespace BLL.Services.Identity;

public class AppRoleService : BaseEntityService<DAL.DTO.Identity.AppRole, DTO.Identity.AppRole, IAppRoleRepository>, IAppRoleService
{
    public AppRoleService(IAppUnitOfWork unitOfWork, IAppRoleRepository repository, IMapper mapper) : 
        base(unitOfWork, repository, new DalBllMapper<AppRole, DTO.Identity.AppRole>(mapper))
    {
        
    }
}
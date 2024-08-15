using AutoMapper;
using Base;
using DAL.Interfaces.Identity;
using Domain.Identity;

namespace DAL.EF.Repositories.Identity;

public class AppRoleRepository : BaseEntityRepository<Domain.Identity.AppRole,DTO.Identity.AppRole,AppDbContext>
    , IAppRoleRepository

{
    public AppRoleRepository(AppDbContext dbContext,IMapper dalMapper) 
        : base(dbContext, new DomainDalMapper<AppRole, DTO.Identity.AppRole>(dalMapper))
    {
    }
}
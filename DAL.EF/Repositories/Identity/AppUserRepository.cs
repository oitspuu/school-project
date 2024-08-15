using AutoMapper;
using Base;
using DAL.Interfaces.Identity;
using Domain.Identity;

namespace DAL.EF.Repositories.Identity;

public class AppUserRepository : BaseEntityRepository<Domain.Identity.AppUser,DTO.Identity.AppUser,AppDbContext>
    ,IAppUserRepository
{
    public AppUserRepository(AppDbContext dbContext, IMapper dalMapper) 
        : base(dbContext, new DomainDalMapper<AppUser, DTO.Identity.AppUser>(dalMapper))
    {
    }
    
}
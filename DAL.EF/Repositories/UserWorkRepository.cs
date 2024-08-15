using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class UserWorkRepository : BaseEntityRepository<Domain.UserWork, DAL.DTO.UserWork, AppDbContext>, IUserWorkRepository
{
    public UserWorkRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<UserWork, DTO.UserWork>(mapper))
    {
        
    }
}
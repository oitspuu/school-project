using AutoMapper;
using Base;
using Base.Helpers;
using DAL.Interfaces;
using Domain;
using UserHobby = DAL.DTO.UserHobby;

namespace DAL.EF.Repositories;

public class HobbyRepository : BaseEntityRepository<Domain.Hobby, DAL.DTO.Hobby, AppDbContext>, IHobbyRepository
{
    public HobbyRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<Hobby, DTO.Hobby>(mapper))
    {
        
    }
}
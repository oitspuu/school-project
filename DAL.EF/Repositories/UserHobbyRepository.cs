using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;


namespace DAL.EF.Repositories;

public class UserHobbyRepository : BaseEntityRepository<Domain.UserHobby, DAL.DTO.UserHobby, AppDbContext>, IUserHobbyRepository
{
    public UserHobbyRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<UserHobby, DTO.UserHobby>(mapper))
    {
        
    }
}
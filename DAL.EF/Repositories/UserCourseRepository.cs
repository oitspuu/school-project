using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class UserCourseRepository : BaseEntityRepository<Domain.UserCourse, DAL.DTO.UserCourse, AppDbContext>, IUserCourseRepository
{
    public UserCourseRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<UserCourse, DTO.UserCourse>(mapper))
    {
        
    }
}
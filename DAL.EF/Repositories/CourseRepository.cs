using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class CourseRepository : BaseEntityRepository<Domain.Course, DAL.DTO.Course, AppDbContext>, ICourseRepository
{
    public CourseRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<Course, DTO.Course>(mapper))
    {
        
    }
}
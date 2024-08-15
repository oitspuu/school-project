using AutoMapper;
using Base;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;

namespace BLL.Services;

public class CourseService : BaseEntityService<DAL.DTO.Course, BLL.DTO.Course, ICourseRepository>, ICourseService
{
    public CourseService(IAppUnitOfWork unitOfWork, ICourseRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<Course, DTO.Course>(mapper))
    {
        
    }
}
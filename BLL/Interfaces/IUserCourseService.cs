using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces;

public interface IUserCourseService : IEntityRepository<BLL.DTO.UserCourse>
{
    Task<BLL.DTO.UserCourse?> GetUserCourseAsync(Guid appUser, Guid userCourse, string language);
    Task<ICollection<BLL.DTO.UserCourse>> GetAllUserCoursesAsync(Guid appUser, string language);
    Task<DTO.UserCourse?> AddUserCourse(DTO.UserCourse entity);
    Task<DTO.UserCourse?> UpdateUserCourse(DTO.UserCourse entity);

    Task<bool> AddTime(Guid appUser, Guid userCourse, TimeSpan time);
}
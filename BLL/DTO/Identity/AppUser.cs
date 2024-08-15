using System;
using System.Collections.Generic;
using Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BLL.DTO.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public ICollection<SleepDuration>? SleepDurations { get; set; }
    public ICollection<UserCourse>? UserCourses { get; set; }
    public ICollection<UserHobby>? UserHobbies { get; set; }
    public ICollection<UserWork>? UserWorks { get; set; }
    
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
    public ICollection<AppRole>? AppRoles { get; set; }
    
}
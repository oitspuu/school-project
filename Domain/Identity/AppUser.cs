using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    [MaxLength(128)] public string? FirstName { get; set; }
    [MaxLength(128)] public string? LastName { get; set; }
    
    public ICollection<UserCourse>? UserCourses { get; set; }
    public ICollection<SleepDuration>? Sleeps { get; set; }
    public ICollection<UserWork>? UserWorkplaces { get; set; }
    public ICollection<UserHobby>? UserHobbies { get; set; }
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}
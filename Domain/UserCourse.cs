using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Identity;

namespace Domain;

public class UserCourse : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid CourseId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserCourse), Name = nameof(Course))]
    public Course? Course { get; set; }    
    
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserCourse), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.UserCourse), Name = nameof(HomeworkTime))]
    public TimeSpan HomeworkTime { get; set; }
}
using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class UserCourse : BaseEntity, IDomainEntityId, IDomainAppUserId<Guid>
{
    public Guid CourseId { get; set; } 
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserCourse), Name = nameof(HomeworkTime))]
    public TimeSpan HomeworkTime { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(CourseName))]
    public string CourseName { get; set; } = default!;
    
    public Guid? OriginalCourseTextId { get; set; }
    public string? Language { get; set; }
    
    public Guid? SchoolId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.School), Name = "Name")]
    public string? SchoolName { get; set; }
    public Guid? OriginalSchoolTextId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(ECTS))]
    public int ECTS { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(Teacher))]
    public string? Teacher { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(StartDate))]
    public DateOnly StartDate { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(EndDate))]
    public DateOnly EndDate { get; set; }
    
}
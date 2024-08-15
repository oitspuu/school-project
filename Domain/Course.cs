using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Translations;

namespace Domain;

public class Course : BaseEntity, IDomainEntityId
{
    [MaxLength(128), Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(CourseName))] 
    public string CourseName { get; set; } = default!;
    
    public Guid? OriginalTextId { get; set; }
    public OriginalText? NameTranslations { get; set; }

    public Guid? SchoolId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(School))]
    public School? School { get; set; }

    public ICollection<UserCourse>? UserCourses { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(ECTS))]
    public int ECTS { get; set; }
    [MaxLength(128), Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(Teacher))]
    public string? Teacher { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(StartDate))]
    public DateOnly StartDate { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Course), Name = nameof(EndDate))]
    public DateOnly EndDate { get; set; }
}
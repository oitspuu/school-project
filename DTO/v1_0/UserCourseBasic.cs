using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class UserCourseBasic: BaseEntity, IDomainEntityId
{
    public Guid CourseId { get; set; } 
    
    public TimeSpan HomeworkTime { get; set; }
    
    public string CourseName { get; set; } = default!;
    
    public string? Language { get; set; }
    
    public int ECTS { get; set; }

    public DateOnly StartDate { get; set; }
 
    public DateOnly EndDate { get; set; }
}
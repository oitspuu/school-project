using System;

namespace DTO.v1_0;

public class UserCourseCreate
{
    public TimeSpan HomeworkTime { get; set; } = TimeSpan.Zero;
    
    public string CourseName { get; set; } = default!;
    
    public string? Language { get; set; }
    
    public string? SchoolName { get; set; }
    
    public int ECTS { get; set; }

    public string? Teacher { get; set; }

    public DateOnly StartDate { get; set; }
 
    public DateOnly EndDate { get; set; }
}
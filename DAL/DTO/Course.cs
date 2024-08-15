using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO;

public class Course : BaseEntity, IDomainEntityId
{

    public string CourseName { get; set; } = default!;
    
    public Guid? OriginalTextId { get; set; }
    
    public Guid? SchoolId { get; set; }
    
    public int ECTS { get; set; }

    public string? Teacher { get; set; }

    public DateOnly StartDate { get; set; }
 
    public DateOnly EndDate { get; set; }
}
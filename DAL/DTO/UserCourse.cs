using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO;

public class UserCourse : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid CourseId { get; set; } 
    public Guid AppUserId { get; set; }
    public TimeSpan HomeworkTime { get; set; }
}
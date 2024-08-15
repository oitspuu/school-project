using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO;

public class WorkHours : BaseEntity, IDomainEntityId
{
    public Guid UserWorkId { get; set; } 
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool LunchBreak { get; set; }
}
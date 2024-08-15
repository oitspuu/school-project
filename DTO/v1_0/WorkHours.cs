using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class WorkHours: BaseEntity, IDomainEntityId
{
    public Guid UserWorkId { get; set; } 
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public bool LunchBreak { get; set; }
}
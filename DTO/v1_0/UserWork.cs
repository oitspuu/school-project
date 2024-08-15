using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class UserWork: BaseEntity, IDomainEntityId
{
    public Guid WorkId { get; set; }
    public string WorkName { get; set; } = default!;
    public TimeSpan LunchBreakDuration { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public TimeSpan TotalWorkHours { get; set; }
}
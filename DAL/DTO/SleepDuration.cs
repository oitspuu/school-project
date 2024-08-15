using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO;

public class SleepDuration : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public DateOnly Day { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}
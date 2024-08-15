using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class Sleep: BaseEntity, IDomainEntityId
{
    public DateOnly Day { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
    public TimeSpan Total { get; set; }
}
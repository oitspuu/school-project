using System;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class Work : BaseEntity, IDomainEntityId
{
    public string WorkName { get; set; } = default!;
    public TimeSpan LunchBreakDuration { get; set; }
}
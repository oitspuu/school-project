using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class WorkHours : BaseEntity, IDomainEntityId
{
    public Guid UserWorkId { get; set; } 
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(Date))]

    public DateOnly Date { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(StartTime))]

    public TimeOnly StartTime { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(EndTime))]

    public TimeOnly EndTime { get; set; }
    [Display(ResourceType = typeof(Resources.BLL.Common), Name = nameof(Duration))]

    public TimeSpan Duration { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(LunchBreak))]

    
    public bool LunchBreak { get; set; }
}
using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace Domain;

public class WorkHours : BaseEntity, IDomainEntityId
{
    public Guid UserWorkId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(UserWork))]
    public UserWork? UserWork { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(Date))]
    public DateOnly Date { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(StartTime))]
    public TimeOnly StartTime { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(EndTime))]
    public TimeOnly EndTime { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.WorkHours), Name = nameof(LunchBreak))]
    public bool LunchBreak { get; set; }
}
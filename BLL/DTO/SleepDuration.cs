using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class SleepDuration : BaseEntity, IDomainEntityId, IDomainAppUserId<Guid>
{
    public Guid AppUserId { get; set; }
    

    [Display(ResourceType = typeof(Resources.BLL.SleepDuration), Name = nameof(Day))]
    public DateOnly Day { get; set; }
    
    [Display(ResourceType = typeof(Resources.BLL.SleepDuration), Name = nameof(Start))]
    public TimeOnly Start { get; set; }

    [Display(ResourceType = typeof(Resources.BLL.SleepDuration), Name = nameof(End))]
    public TimeOnly End { get; set; }
    

    [Display(ResourceType = typeof(Resources.BLL.SleepDuration), Name = nameof(Total))]
    public TimeSpan Total { get; set; }
}
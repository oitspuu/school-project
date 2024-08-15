using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class UserWork : BaseEntity, IDomainEntityId, IDomainAppUserId<Guid>
{
    public Guid AppUserId { get; set; }
    public Guid WorkId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Work), Name = nameof(WorkName))]
    public string WorkName { get; set; } = default!;
    [Display(ResourceType = typeof(Resources.Domain.Work), Name = nameof(LunchBreakDuration))]
    public TimeSpan LunchBreakDuration { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(Start))]
    public DateOnly Start { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(End))]

    public DateOnly End { get; set; }
    [Display(ResourceType = typeof(Resources.BLL.Common), Name = nameof(TotalWorkHours))]

    public TimeSpan TotalWorkHours { get; set; }
    public IEnumerable<WorkHours>? WorkHours { get; set; }
}
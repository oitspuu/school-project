using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace Domain;

public class Work : BaseEntity, IDomainEntityId
{
    [MaxLength(128), Display(ResourceType = typeof(Resources.Domain.Work), Name = nameof(WorkName))]
    public string WorkName { get; set; } = default!;
    [Display(ResourceType = typeof(Resources.Domain.Work), Name = nameof(LunchBreakDuration))]
    public TimeSpan LunchBreakDuration { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Work), Name = nameof(UserWorkHours))]
    public ICollection<UserWork>? UserWorkHours { get; set; }
}
using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Identity;

namespace Domain;

public class SleepDuration : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.SleepDuration), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.SleepDuration), Name = nameof(Day))]
    public DateOnly Day { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.SleepDuration), Name = nameof(Start))]
    public TimeOnly Start { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.SleepDuration), Name = nameof(End))]
    public TimeOnly End { get; set; }
}
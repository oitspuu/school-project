using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Identity;

namespace Domain;

public class UserWork : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }

    public Guid WorkId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(Work))]
    public Work? Work { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(WorkHours))]

    public ICollection<WorkHours>? Hours { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(Start))]
    public DateOnly Start { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserWork), Name = nameof(End))]
    public DateOnly End { get; set; } = DateOnly.MaxValue;
}
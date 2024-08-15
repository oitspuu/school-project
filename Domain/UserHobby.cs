using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Identity;

namespace Domain;

public class UserHobby : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserHobby), Name = nameof(AppUser))]
    public AppUser? AppUser { get; set; }
    
    public Guid HobbyId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserHobby), Name = nameof(Hobby))]
    public Hobby? Hobby { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.UserHobby), Name = nameof(TimeSpent))]
    public TimeSpan TimeSpent { get; set; }
}
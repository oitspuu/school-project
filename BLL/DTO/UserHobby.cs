using System;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class UserHobby : BaseEntity, IDomainEntityId, IDomainAppUserId<Guid>
{
    public Guid AppUserId { get; set; }
    public Guid HobbyId { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.UserHobby), Name = nameof(TimeSpent))]
    public TimeSpan TimeSpent { get; set; }
    [Display(ResourceType = typeof(Resources.Domain.Hobby), Name = nameof(HobbyName))]
    public string HobbyName { get; set; } = default!;
    public Guid? OriginalTextId { get; set; }
    public string? Language { get; set; }
}
using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO;

public class UserHobby : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public Guid HobbyId { get; set; }
    public TimeSpan TimeSpent { get; set; }
}
using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class UserHobby: BaseEntity, IDomainEntityId
{
    public Guid HobbyId { get; set; }
    public TimeSpan TimeSpent { get; set; }
    public string HobbyName { get; set; } = default!;
    public string? Language { get; set; }
}
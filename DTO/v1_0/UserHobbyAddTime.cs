using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class UserHobbyAddTime : BaseEntity, IDomainEntityId
{
    public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;
}
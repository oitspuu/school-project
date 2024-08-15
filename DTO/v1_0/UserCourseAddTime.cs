using System;
using Base;
using Base.Interfaces;

namespace DTO.v1_0;

public class UserCourseAddTime : BaseEntity, IDomainEntityId
{
    public TimeSpan TimeSpent { get; set; } = TimeSpan.Zero;
}
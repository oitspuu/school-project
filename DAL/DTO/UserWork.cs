using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO;

public class UserWork : BaseEntity, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public Guid WorkId { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
}
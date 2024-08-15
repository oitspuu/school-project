using System;
using Base;
using Base.Interfaces;

namespace Domain.Identity;

public class RefreshToken : BaseRefreshToken, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
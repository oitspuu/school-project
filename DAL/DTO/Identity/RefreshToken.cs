using System;
using Base;
using Base.Interfaces;

namespace DAL.DTO.Identity;

public class RefreshToken : BaseRefreshToken, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
}
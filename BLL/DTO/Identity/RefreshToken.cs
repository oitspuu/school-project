using System;
using Base;
using Base.Interfaces;

namespace BLL.DTO.Identity;

public class RefreshToken : BaseRefreshToken, IDomainAppUserId<Guid>, IDomainEntityId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
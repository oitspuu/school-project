using System;
using Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppRole : IdentityRole<Guid>, IDomainEntityId
{
    
}
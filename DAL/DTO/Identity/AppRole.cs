using Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.DTO.Identity;

public class AppRole : IdentityRole<Guid>, IDomainEntityId
{
    
}
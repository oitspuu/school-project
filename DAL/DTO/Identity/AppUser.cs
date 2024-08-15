using Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.DTO.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    
}
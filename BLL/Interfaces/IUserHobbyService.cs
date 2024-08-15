using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces;

public interface IUserHobbyService : IEntityRepository<BLL.DTO.UserHobby>
{
    Task<BLL.DTO.UserHobby?> GetUserHobbyAsync(Guid appUser, Guid userHobby, string language); 
    Task<ICollection<BLL.DTO.UserHobby>> GetAllUserHobbiesAsync(Guid appUser, string language);
    Task<bool> AddTime(Guid appUser, Guid userHobby, TimeSpan time);

    Task<DTO.UserHobby?> AddUserHobby(DTO.UserHobby entity);
    Task<DTO.UserHobby?> UpdateUserHobby(DTO.UserHobby entity);
}
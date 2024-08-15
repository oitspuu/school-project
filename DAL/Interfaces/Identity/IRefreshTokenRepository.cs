using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace DAL.Interfaces.Identity;

public interface IRefreshTokenRepository : IEntityRepository<DTO.Identity.RefreshToken>
{
    public Task<int> RemoveExpiredAsync(Guid userId);
    public Task<IEnumerable<DAL.DTO.Identity.RefreshToken>> GetNotExpiredAsync(Guid userId, string token,bool noTracking = true);
    public Task<int> RemoveAfterLogoutAsync(Guid userId, string token);
    
}
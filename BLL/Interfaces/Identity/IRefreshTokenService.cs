using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces.Identity;

public interface IRefreshTokenService : IEntityRepository<BLL.DTO.Identity.RefreshToken>
{
    public Task<int> RemoveExpiredAsync(Guid userId);

    public Task<IEnumerable<DTO.Identity.RefreshToken>> GetNotExpiredAsync(Guid userId, string token,
        bool noTracking = true);
    public Task<int> RemoveAfterLogoutAsync(Guid userId, string token);
}
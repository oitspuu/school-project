using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base;
using DAL.Interfaces.Identity;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories.Identity;

public class RefreshTokenRepository : BaseEntityRepository<Domain.Identity.RefreshToken, DAL.DTO.Identity.RefreshToken, AppDbContext>,
    IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DomainDalMapper<RefreshToken, DTO.Identity.RefreshToken>(mapper))
    {
        
    }

    public async Task<int> RemoveExpiredAsync(Guid userId)
    {
        var list = await CreateQuery(userId).Where(token => token.Expires < DateTime.UtcNow).ToListAsync();
        var count = 0;
        foreach (var rt in list)
        {
            RepoDbSet.Remove(rt);
            count++;
        }
        return count;
    }

    public async Task<IEnumerable<DTO.Identity.RefreshToken>> GetNotExpiredAsync(Guid userId, string token, bool noTracking = true)
    {
        return (await GetAllAsync(userId)).Where(rt => 
            (rt.RefreshToken == token && rt.Expires > DateTime.UtcNow) || 
            (rt.PreviousRefreshToken == token && rt.PreviousExpires > DateTime.Now));
    }

    public async Task<int> RemoveAfterLogoutAsync(Guid userId, string token)
    {
        var list = await CreateQuery(userId).Where(rt =>
            rt.RefreshToken == token || rt.PreviousRefreshToken == token).ToListAsync();
        var count = 0;
        foreach (var refresh in list)
        {
            RepoDbSet.Remove(refresh);
            count++;
        }

        return count;
    }
}
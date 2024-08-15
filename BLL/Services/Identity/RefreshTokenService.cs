using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base;
using BLL.Interfaces.Identity;
using DAL;
using DAL.DTO.Identity;
using DAL.Interfaces.Identity;

namespace BLL.Services.Identity;

public class RefreshTokenService : BaseEntityService<DAL.DTO.Identity.RefreshToken, DTO.Identity.RefreshToken, IRefreshTokenRepository>, 
    IRefreshTokenService
{
    private readonly IMapper _mapper;

    public RefreshTokenService(IAppUnitOfWork unitOfWork, IRefreshTokenRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<RefreshToken, DTO.Identity.RefreshToken>(mapper))
    {
        _mapper = mapper;
    }
    

    public async Task<int> RemoveExpiredAsync(Guid userId)
    {
        return await Repository.RemoveExpiredAsync(userId);
    }

    public async Task<IEnumerable<DTO.Identity.RefreshToken>> GetNotExpiredAsync(Guid userId, string token, bool noTracking = true)
    {
        return (await Repository.GetNotExpiredAsync(userId, token))
            .Select(rt => Mapper.Map(rt)).OfType<DTO.Identity.RefreshToken>();
    }

    public async Task<int> RemoveAfterLogoutAsync(Guid userId, string token)
    {
        return await Repository.RemoveAfterLogoutAsync(userId, token);
    }
}
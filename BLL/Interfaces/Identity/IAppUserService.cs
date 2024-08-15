using System;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces.Identity;

public interface IAppUserService : IEntityRepository<BLL.DTO.Identity.AppUser>
{
    Task<BLL.DTO.Identity.AppUser?> FirstOrDefaultWithCollectionsAsync(Guid entityId, 
        string language = Base.Helpers.Constants.English,bool noTracking = true);
}
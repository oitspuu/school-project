using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces;

public interface IUserWorkService : IEntityRepository<BLL.DTO.UserWork>
{
    Task<DTO.UserWork?> GetUserWorkAsync(Guid appUser, Guid userWork);
    Task<DTO.UserWork?> GetUserWorkWithHoursAsync(Guid appUser, Guid userWork);
    Task<IEnumerable<DTO.UserWork>> GetAllUserWorkAsync(Guid appUser);
    DTO.UserWork? AddUserWork(DTO.UserWork entity);
    Task<DTO.UserWork?> UpdateUserWork(DTO.UserWork entity);
    
}
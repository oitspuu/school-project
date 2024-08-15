using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces;

public interface ISleepService : IEntityRepository<BLL.DTO.SleepDuration>
{
    Task<BLL.DTO.SleepDuration?>GetSleepWithTotalAsync(Guid entityId, Guid userId);
    Task<IEnumerable<BLL.DTO.SleepDuration>> GetAllSleepWithTotalAsync(Guid userId);
}
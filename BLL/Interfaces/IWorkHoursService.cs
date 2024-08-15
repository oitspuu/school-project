using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Base.Interfaces;

namespace BLL.Interfaces;

public interface IWorkHoursService : IEntityRepository<BLL.DTO.WorkHours>
{
    Task<BLL.DTO.WorkHours?> GetWorkDayAsync(Guid appUserId, Guid id);
    Task<IEnumerable<BLL.DTO.WorkHours>> GetWorkDaysAsync(Guid appUserId, Guid userWorkId);
    
}
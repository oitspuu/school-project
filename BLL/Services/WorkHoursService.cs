using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;

namespace BLL.Services;

public class WorkHoursService : BaseEntityService<DAL.DTO.WorkHours, BLL.DTO.WorkHours, IWorkHoursRepository>, IWorkHoursService
{
    private readonly IAppUnitOfWork _unitOfWork;

    public WorkHoursService(IAppUnitOfWork unitOfWork, IWorkHoursRepository repository, IMapper mapper) : 
        base(unitOfWork, repository, new DalBllMapper<WorkHours, DTO.WorkHours>(mapper))
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DTO.WorkHours?> GetWorkDayAsync(Guid appUserId, Guid id)
    {
        var day = Mapper.Map(await Repository.FirstOrDefaultAsync(id, appUserId));
        if (day == null) return null;

        if (!day.LunchBreak) return CalculateTotal(day);

        var workId = (await _unitOfWork.UserWorkRepository.FirstOrDefaultAsync(day.UserWorkId))?.WorkId;
        if (workId == null) return CalculateTotal(day);;

        var lunch = (await _unitOfWork.WorkRepository.FirstOrDefaultAsync((Guid) workId))?.LunchBreakDuration;
        return CalculateTotal(day, lunch ?? TimeSpan.Zero);
    }

    public async Task<IEnumerable<DTO.WorkHours>> GetWorkDaysAsync(Guid appUserId, Guid userWorkId)
    {
        var hours = (await Repository.GetAllAsync(appUserId))
            .Where(h => h.UserWorkId == userWorkId)
            .Select(h => Mapper.Map(h))
            .OfType<DTO.WorkHours>().ToArray();
        
        foreach (var hour in hours)
        {
            var workId = (await _unitOfWork.UserWorkRepository.FirstOrDefaultAsync(hour.UserWorkId))?.WorkId;
            if (workId == null || hour.LunchBreak == false)
            {
                hour.Duration = CalculateTotal(hour).Duration;
                continue;
            }
            var lunch = (await _unitOfWork.WorkRepository.FirstOrDefaultAsync((Guid) workId))?.LunchBreakDuration;
            hour.Duration = CalculateTotal(hour, lunch ?? TimeSpan.Zero).Duration;
        }

        return hours;
    }

    private static DTO.WorkHours CalculateTotal(DTO.WorkHours hours, TimeSpan lunch = default)
    {
        var end = hours.EndTime;
        var start = hours.StartTime;
        
        if (end > start)
        {
            hours.Duration += (end - start);
        }
        else
        {
            hours.Duration += new TimeSpan(24, 0, 0) - start.ToTimeSpan() + end.ToTimeSpan();
        }

        if (lunch != default)
        {
            hours.Duration -= lunch;
        }
        return hours;
    }
}
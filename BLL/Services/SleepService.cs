using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;

namespace BLL.Services;

public class SleepService : BaseEntityService<DAL.DTO.SleepDuration, BLL.DTO.SleepDuration, ISleepRepository>, ISleepService
{
    public SleepService(IAppUnitOfWork unitOfWork, ISleepRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<SleepDuration, DTO.SleepDuration>(mapper))
    {

    }


    public async Task<DTO.SleepDuration?> GetSleepWithTotalAsync(Guid entityId, Guid userId)
    {
        var sleep = Mapper.Map(await Repository.FirstOrDefaultAsync(entityId, userId));

        if (sleep == null) return sleep;

        sleep.Total = CalculateTotal(sleep.Start, sleep.End);

        return sleep;
    }

    public async Task<IEnumerable<DTO.SleepDuration>> GetAllSleepWithTotalAsync(Guid userId)
    {
        return (await Repository.GetAllAsync(userId))
            .Select(s => Mapper.Map(s)).OfType<DTO.SleepDuration>()
            .Select(s =>
            { 
                s.Total = CalculateTotal(s.Start, s.End);
                return s;
            })
            .OrderByDescending(s => s.Day);
    }

    private static TimeSpan CalculateTotal(TimeOnly start, TimeOnly end)
    {
        TimeSpan total;
        if (end > start)
        {
            total = end - start;
        }
        else
        {
            var helper = new TimeSpan(24, 0, 0);
            total =  helper - start.ToTimeSpan() + end.ToTimeSpan();
        }
        
        return total;
    }
}
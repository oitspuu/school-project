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

public class UserWorkService : BaseEntityService<DAL.DTO.UserWork, BLL.DTO.UserWork, IUserWorkRepository>, IUserWorkService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserWorkService(IAppUnitOfWork unitOfWork, IUserWorkRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<UserWork, DTO.UserWork>(mapper))
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DTO.UserWork?> GetUserWorkAsync(Guid appUser, Guid id)
    {
        var userWork = Mapper.Map(await Repository.FirstOrDefaultAsync(id, appUser));
        if (userWork == null) return null;

        var work = await _unitOfWork.WorkRepository.FirstOrDefaultAsync(userWork.WorkId);
        if (work == null) return userWork;
        
        userWork.WorkName = work.WorkName;
        userWork.LunchBreakDuration = work.LunchBreakDuration;

        return userWork;
    }

    public async Task<DTO.UserWork?> GetUserWorkWithHoursAsync(Guid appUser, Guid id)
    {
        var userWork = await GetUserWorkAsync(appUser, id);
        if (userWork == null) return null;

        return await AddWorkDaysAsync(appUser, userWork);
    }

    public async Task<IEnumerable<DTO.UserWork>> GetAllUserWorkAsync(Guid appUser)
    {
        var list = (await Repository.GetAllAsync(appUser))
            .Select(u => Mapper.Map(u))
            .OfType<DTO.UserWork>()
            .ToArray();

        foreach (var userWork in list)
        {
            var work = await _unitOfWork.WorkRepository.FirstOrDefaultAsync(userWork.WorkId);
            if (work != null)
            {
                userWork.WorkName = work.WorkName;
                userWork.LunchBreakDuration = work.LunchBreakDuration;
            }

            userWork.TotalWorkHours = (await AddWorkDaysAsync(appUser, userWork, false)).TotalWorkHours;
        }
        return list;
    }

    public DTO.UserWork? AddUserWork(DTO.UserWork entity)
    {
        var id = Guid.NewGuid();
        var workId = Guid.NewGuid();

        var work = new Work()
        {
            Id = workId,
            LunchBreakDuration = entity.LunchBreakDuration,
            WorkName = entity.WorkName
        };
        
        var addedWork = _unitOfWork.WorkRepository.Add(work);
        if (addedWork == null) return null;

        entity.Id = id;
        entity.WorkId = workId;
        var addedUserWork = Repository.Add(Mapper.Map(entity));
        return addedUserWork == null ? null : entity;
    }

    public async Task<DTO.UserWork?> UpdateUserWork(DTO.UserWork entity)
    {
        var original = await Repository.FirstOrDefaultAsync(entity.Id);
        if (original == null) return null;

        original.Start = entity.Start;
        original.End = entity.End;

        Repository.Update(original);

        var work = await _unitOfWork.WorkRepository.FirstOrDefaultAsync(entity.WorkId);
        if (work == null) return null;

        work.WorkName = entity.WorkName;
        work.LunchBreakDuration = entity.LunchBreakDuration;

        _unitOfWork.WorkRepository.Update(work);
        
        return entity;
    }

    private async Task<DTO.UserWork> AddWorkDaysAsync(Guid appUserId,
        DTO.UserWork userWork, bool withList = true)
    {
        var userWorkHoursMapper = new DalBllMapper<DTO.WorkHours, DAL.DTO.WorkHours>(_mapper);
        
        var hours = (await _unitOfWork.WorkHoursRepository.GetAllAsync(appUserId))
            .Where(h => h.UserWorkId == userWork.Id)
            .Select(h => userWorkHoursMapper.Map(h))
            .OfType<DTO.WorkHours>().ToArray();
        
        foreach (var hour in hours)
        {
            var workId = (await _unitOfWork.UserWorkRepository.FirstOrDefaultAsync(hour.UserWorkId))?.WorkId;
            if (workId == null || hour.LunchBreak == false)
            {
                hour.Duration = CalculateTotal(hour).Duration;
                userWork.TotalWorkHours += hour.Duration;
                continue;
            }
            var lunch = (await _unitOfWork.WorkRepository.FirstOrDefaultAsync((Guid) workId))?.LunchBreakDuration;
            hour.Duration = CalculateTotal(hour, lunch ?? TimeSpan.Zero).Duration;
            userWork.TotalWorkHours += hour.Duration;
        }

        if (withList)
        {
            userWork.WorkHours = hours;
        }
        
        return userWork;
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
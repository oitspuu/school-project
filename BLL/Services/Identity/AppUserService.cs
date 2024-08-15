using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base.Helpers;
using BLL.DTO;
using BLL.Interfaces.Identity;
using DAL;
using DAL.DTO.Identity;
using DAL.Interfaces.Identity;

namespace BLL.Services.Identity;

public class AppUserService : BaseEntityService<DAL.DTO.Identity.AppUser, DTO.Identity.AppUser, IAppUserRepository>, IAppUserService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private IMapper _mapper;
    public AppUserService(IAppUnitOfWork unitOfWork, IAppUserRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<AppUser, DTO.Identity.AppUser>(mapper))
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<DTO.Identity.AppUser?> FirstOrDefaultWithCollectionsAsync(Guid entityId, string language = Constants.English, bool noTracking = true)
    {
        var user = await FirstOrDefaultAsync(entityId);
        if (user == null) return default;
        
        var sleepMapper = new DalBllMapper<SleepDuration, DAL.DTO.SleepDuration>(_mapper);
        user.SleepDurations = (await _unitOfWork.SleepRepository.GetAllAsync(entityId))
            .Select(s => sleepMapper.Map(s)).OfType<SleepDuration>().ToList()
            .OrderByDescending(s => s.Day).Take(10).ToList();

        user.UserCourses = await GetUserCoursesAsync(entityId, language);
        user.UserHobbies = await GetUserHobbiesAsync(entityId, language);
        user.UserWorks = await GetUserWorkplacesAsync(entityId);
        
        return user;
    }

    private async Task<ICollection<UserCourse>> GetUserCoursesAsync(Guid appUser, string language)
    {
        var userCourseMapper = new DalBllMapper<UserCourse, DAL.DTO.UserCourse>(_mapper);
        var userCourses = (await _unitOfWork.UserCourseRepository.GetAllAsync(appUser))
            .Select(u => userCourseMapper.Map(u)).OfType<UserCourse>().ToList()
            .OrderByDescending(c => c.HomeworkTime).Take(10).ToList();

        foreach (var userCourse in userCourses)
        {
            var course = await _unitOfWork.CourseRepository.FirstOrDefaultAsync(userCourse.CourseId);
            if (course == null) continue;

            userCourse.CourseName = course.CourseName;
            userCourse.ECTS = course.ECTS;
            userCourse.StartDate = course.StartDate;
            userCourse.EndDate = course.EndDate;
            
            if (language is not (Constants.English or Constants.Estonian)) continue;

            userCourse.Language = language;
            userCourse.OriginalCourseTextId = course.OriginalTextId;
            
            if (userCourse.OriginalCourseTextId == null) continue;
            userCourse.CourseName = await GetTranslation((Guid) userCourse.OriginalCourseTextId, language) ?? course.CourseName;
        }
        return userCourses;
    }

    private async Task<ICollection<UserHobby>> GetUserHobbiesAsync(Guid appUser, string language)
    {
        var userHobbyMapper = new DalBllMapper<UserHobby, DAL.DTO.UserHobby>(_mapper);
        var userHobbies = (await _unitOfWork.UserHobbyRepository.GetAllAsync(appUser))
            .Select(u => userHobbyMapper.Map(u)).OfType<UserHobby>().ToList()
            .OrderByDescending(u => u.TimeSpent).Take(10).ToList();

        foreach (var userHobby in userHobbies)
        {
            var hobby = await _unitOfWork.HobbyRepository.FirstOrDefaultAsync(userHobby.HobbyId);
            if (hobby == null) continue;
            userHobby.HobbyName = hobby.HobbyName;
            userHobby.OriginalTextId = hobby.OriginalTextId;

            if (language is not (Constants.English or Constants.Estonian) || userHobby.OriginalTextId is null) continue;

            userHobby.HobbyName = await GetTranslation((Guid) userHobby.OriginalTextId, language) ?? hobby.HobbyName;
        }

        return userHobbies;
    }

    private async Task<ICollection<UserWork>> GetUserWorkplacesAsync(Guid appUser)
    {
        var userWorkPlaceMapper = new DalBllMapper<UserWork, DAL.DTO.UserWork>(_mapper);
        var userWorkPlaces = (await _unitOfWork.UserWorkRepository.GetAllAsync(appUser))
            .Select(w => userWorkPlaceMapper.Map(w)).OfType<UserWork>().ToList()
            .OrderByDescending(w => w.Start).Take(10).ToList();
        
        var userWorkHoursMapper = new DalBllMapper<WorkHours, DAL.DTO.WorkHours>(_mapper);

        foreach (var userWork in userWorkPlaces)
        {
            var work = await _unitOfWork.WorkRepository.FirstOrDefaultAsync(userWork.WorkId);
            if (work == null) continue;
            userWork.WorkName = work.WorkName;
            userWork.LunchBreakDuration = work.LunchBreakDuration;

            var workHours = (await _unitOfWork.WorkHoursRepository.GetAllAsync()).Where(h => h.UserWorkId == userWork.Id)
                .Select(w => userWorkHoursMapper.Map(w)).OfType<WorkHours>();
            foreach (var workHour in workHours)
            {
                var start = workHour.StartTime;
                var end = workHour.EndTime;

                if (end > start)
                {
                    userWork.TotalWorkHours += (end - start);
                }
                else
                {
                    userWork.TotalWorkHours += new TimeSpan(24, 0, 0) - start.ToTimeSpan() + end.ToTimeSpan();
                }

                if (workHour.LunchBreak)
                {
                    userWork.TotalWorkHours -= userWork.LunchBreakDuration;
                }
            }
        }

        return userWorkPlaces;
    }
    
    private async Task<string?> GetTranslation(Guid textId, string language)
    {
        var languageId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(language);
        return languageId == null ? null : _unitOfWork.TranslationRepository.FindTranslation(textId, (Guid) languageId);
    }
    
}
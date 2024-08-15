using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base;
using Base.Helpers;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.DTO.Translations;
using DAL.Interfaces;

namespace BLL.Services;

public class UserCourseService : BaseEntityService<DAL.DTO.UserCourse, BLL.DTO.UserCourse, IUserCourseRepository>, IUserCourseService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserCourseService(IAppUnitOfWork unitOfWork, IUserCourseRepository repository, IMapper mapper) : 
        base(unitOfWork, repository, new DalBllMapper<UserCourse, DTO.UserCourse>(mapper))
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DTO.UserCourse?> GetUserCourseAsync(Guid appUser, Guid userCourse, string language)
    {
        var thisUserCourse = Mapper.Map(await Repository.FirstOrDefaultAsync(userCourse, appUser));
        if (thisUserCourse == null) return null;

        thisUserCourse = await AddInformation(thisUserCourse, language);

        return thisUserCourse;

    }

    public async Task<ICollection<DTO.UserCourse>> GetAllUserCoursesAsync(Guid appUser, string language)
    {
        var courses = new List<DTO.UserCourse>();
        var userCourses = (await Repository.GetAllAsync(appUser))
            .Select(u => Mapper.Map(u)).OfType<BLL.DTO.UserCourse>().ToList();
        
        foreach (var userCourse in userCourses)
        {
            var course = await AddInformation(userCourse, language);
            courses.Add(course);
        }
        return courses;
    }

    public async Task<DTO.UserCourse?> AddUserCourse(DTO.UserCourse entity)
    {
        entity.Id = Guid.NewGuid();
        entity.CourseId = Guid.NewGuid();

        Guid? langId = null;
        
        if (entity.Language != null)
        {
            langId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(entity.Language);
        }
        
        if (entity.SchoolName != null)
        {
            entity.SchoolId = Guid.NewGuid();
            var schoolMapper = new DalBllMapper<BLL.DTO.School, DAL.DTO.School>(_mapper);
            var school = new BLL.DTO.School()
            {
                Name = entity.SchoolName,
                Id = (Guid) entity.SchoolId,
            };
            if (langId != null)
            {
                entity.OriginalSchoolTextId = CreateNewTranslation((Guid) langId, entity.SchoolName);
                school.OriginalTextId = entity.OriginalSchoolTextId;
            }
            _unitOfWork.SchoolRepository.Add(schoolMapper.Map(school));
        }

        var course = new BLL.DTO.Course()
        {
            Id = entity.CourseId,
            CourseName = entity.CourseName,
            ECTS = entity.ECTS,
            SchoolId = entity.SchoolId,
            Teacher = entity.Teacher,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };

        if (langId != null)
        {
            entity.OriginalCourseTextId = CreateNewTranslation((Guid)langId, entity.CourseName);
            course.OriginalTextId = entity.OriginalCourseTextId;
        }
        var courseMapper = new DalBllMapper<BLL.DTO.Course, DAL.DTO.Course>(_mapper);
        
        if (_unitOfWork.CourseRepository.Add(courseMapper.Map(course)) == null)
        {
            return null;
        }

        return Repository.Add(Mapper.Map(entity)) == null ? null : entity;
    }

    public async Task<DTO.UserCourse?> UpdateUserCourse(DTO.UserCourse entity)
    {
        Guid? schoolTextId = null;
        Guid? courseTextId = null;
        
        var original = await Repository.FirstOrDefaultAsync(entity.Id, entity.AppUserId);
        if (original == null) return null;

        if (entity.Language != null)
        {
            if (entity.SchoolName != null)
            {
                schoolTextId = await SetTranslation(entity.OriginalSchoolTextId, entity.Language, entity.SchoolName);
                
            }
            courseTextId = await SetTranslation(entity.OriginalCourseTextId, entity.Language, entity.CourseName);
        }

        var originalCourse = await _unitOfWork.CourseRepository.FirstOrDefaultAsync(original.CourseId);
        if (originalCourse == null) return null;

        if (entity.SchoolId != null)
        {
            var originalSchool = await _unitOfWork.SchoolRepository.FirstOrDefaultAsync((Guid) entity.SchoolId) ?? new School()
            {
                Id = Guid.NewGuid()
            };
            if (schoolTextId != null && entity.SchoolName != null)
            {
                originalSchool.Name = entity.SchoolName;
                originalSchool.OriginalTextId = schoolTextId;
                entity.SchoolId = originalSchool.Id;
                _unitOfWork.SchoolRepository.Update(originalSchool);
            }
        }

        if (courseTextId != null)
        {
            originalCourse.OriginalTextId = courseTextId;
        }
        
        originalCourse.CourseName = entity.CourseName;
        originalCourse.Teacher = entity.Teacher;
        originalCourse.ECTS = entity.ECTS;
        originalCourse.StartDate = entity.StartDate;
        originalCourse.EndDate = entity.EndDate;
        originalCourse.SchoolId = entity.SchoolId;
        _unitOfWork.CourseRepository.Update(originalCourse);

        original.HomeworkTime = entity.HomeworkTime;
        Repository.Update(original);
        
        return entity;
    }

    private async Task<DTO.UserCourse> AddInformation(DTO.UserCourse userCourse, string language)
    {
        var course = await _unitOfWork.CourseRepository.FirstOrDefaultAsync(userCourse.CourseId);
        if (course == null) return userCourse;
       
        userCourse.CourseName = course.CourseName;
        userCourse.OriginalCourseTextId = course.OriginalTextId;
        userCourse.ECTS = course.ECTS;
        userCourse.Teacher = course.Teacher;
        userCourse.StartDate = course.StartDate;
        userCourse.EndDate = course.EndDate;
            
        if (course.SchoolId != null)
        {
            userCourse.SchoolId = course.SchoolId;
            var school = await _unitOfWork.SchoolRepository.FirstOrDefaultAsync((Guid) course.SchoolId);
            userCourse.SchoolName = school?.Name;
            userCourse.OriginalSchoolTextId = school?.OriginalTextId;
        }
        
        if (language is not (Constants.English or Constants.Estonian)) return userCourse;

        userCourse.Language = language;

        if (userCourse.OriginalCourseTextId != null)
        {
            userCourse.CourseName = await GetTranslation((Guid) userCourse.OriginalCourseTextId,language) ?? course.CourseName;
        }
        if (userCourse.OriginalSchoolTextId != null)
        {
            userCourse.SchoolName = await GetTranslation((Guid) userCourse.OriginalSchoolTextId,language) ?? userCourse.SchoolName;
        }
        return userCourse;
    }
    
    public async Task<bool> AddTime(Guid appUser, Guid userCourse, TimeSpan time)
    {
        var course = await Repository.FirstOrDefaultAsync(userCourse, appUser);
        if (course == null) return false;

        course.HomeworkTime += time;

        var updated = Repository.Update(course);
        return updated != null;
    }
    
    private async Task<string?> GetTranslation(Guid textId, string language)
    {
        var languageId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(language);
        return languageId == null ? null : _unitOfWork.TranslationRepository.FindTranslation(textId, (Guid) languageId);
    }
    
    private async Task<Guid?> SetTranslation(Guid? textId, string language, string translation)
    {
        var languageId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(language);
        if (languageId == null) return null;

        if (textId == null)
        {
            return CreateNewTranslation((Guid)languageId, translation);
        }
        var original = await _unitOfWork.TextRepository.FirstOrDefaultAsync((Guid) textId);
        
        if (original == null)
        {
            return CreateNewTranslation((Guid)languageId, translation);
        }

        if (original.LanguageId == (Guid)languageId)
        {
            original.OriginalText = translation;
        }
        
        var text = _unitOfWork.TranslationRepository.FindTranslationEntity(original.Id, (Guid) languageId);
        UoW.Clear();
        if (text == null)
        {
            var newText = new Translation()
            {
                Id = Guid.NewGuid(),
                LanguageId = (Guid)languageId,
                TextId = original.Id,
                Translation = translation
            };
            _unitOfWork.TranslationRepository.Add(newText);
        }
        else
        {
            text.Translation = translation;
            _unitOfWork.TranslationRepository.Update(text);
        }

        _unitOfWork.TextRepository.Update(original);
        return original.Id;
    }
    
    private Guid CreateNewTranslation(Guid languageId, string translation)
    {
        var id = _unitOfWork.TextRepository.CreateTextForTranslation(languageId, translation);
        _unitOfWork.TranslationRepository.CreateTranslation(languageId, id, translation);
        return id;
    }
    
}
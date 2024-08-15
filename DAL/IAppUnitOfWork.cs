using Base.Interfaces;
using DAL.Interfaces;
using DAL.Interfaces.Identity;
using DAL.Interfaces.Translations;

namespace DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IAppRoleRepository AppRoleRepository { get; }
    IAppUserRepository AppUserRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    
    ITranslationRepository TranslationRepository { get; }
    ILanguageRepository LanguageRepository { get; }
    IOriginalTextRepository TextRepository { get; }
    
    ICourseRepository CourseRepository { get; }
    IHobbyRepository HobbyRepository { get; }
    ISchoolRepository SchoolRepository{ get; }
    ISleepRepository SleepRepository { get; }
    IUserCourseRepository UserCourseRepository { get; }
    IUserHobbyRepository UserHobbyRepository { get; }
    IUserWorkRepository UserWorkRepository { get; }
    IWorkHoursRepository WorkHoursRepository { get; }
    IWorkRepository WorkRepository{ get; }
}
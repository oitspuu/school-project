using Base.Interfaces;
using BLL.Interfaces.Identity;
using BLL.Interfaces.Translations;

namespace BLL.Interfaces;

public interface IAppBll : IBll
{
    IAppRoleService AppRoles { get; }
    IAppUserService AppUsers { get; }
    IRefreshTokenService RefreshTokens { get; }
    
    ILanguageService Languages { get; }
    IOriginalTextService OriginalTexts { get; }
    ITranslationService Translations { get; }
    
    ICourseService Courses { get; }
    IHobbyService Hobbies { get; }
    ISchoolService Schools { get; }
    ISleepService Sleeps { get; }
    IUserCourseService UserCourses { get; }
    IUserHobbyService UserHobbies { get; }
    IUserWorkService UserWorkplaces { get; }
    IWorkHoursService WorkHours { get; }
    IWorkService Workplaces { get; }
}
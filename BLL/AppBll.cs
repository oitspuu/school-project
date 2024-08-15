using AutoMapper;
using Base;
using BLL.Interfaces;
using BLL.Interfaces.Identity;
using BLL.Interfaces.Translations;
using BLL.Services;
using BLL.Services.Identity;
using BLL.Services.Translations;
using DAL;
using DAL.EF;

namespace BLL;

public class AppBll : BaseBll<AppDbContext>, IAppBll
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    private IAppRoleService? _appRoles;
    private IAppUserService? _appUsers;
    private IRefreshTokenService? _refreshTokens;
    private ILanguageService? _languages;
    private IOriginalTextService? _originalTexts;
    private ITranslationService? _translations;
    private ICourseService? _courses;
    private IHobbyService? _hobbies;
    private ISchoolService? _schools;
    private ISleepService? _sleeps;
    private IUserCourseService? _userCourses;
    private IUserHobbyService? _userHobbies;
    private IUserWorkService? _userWorkplaces;
    private IWorkHoursService? _workHours;
    private IWorkService? _workplaces;

    public AppBll(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }

    public IAppRoleService AppRoles => _appRoles ?? new AppRoleService(_uow, _uow.AppRoleRepository, _mapper);

    public IAppUserService AppUsers => _appUsers ?? new AppUserService(_uow, _uow.AppUserRepository, _mapper);

    public IRefreshTokenService RefreshTokens => _refreshTokens ?? new RefreshTokenService(_uow, _uow.RefreshTokenRepository, _mapper);

    public ILanguageService Languages => _languages ?? new LanguageService(_uow, _uow.LanguageRepository, _mapper);

    public IOriginalTextService OriginalTexts => _originalTexts ?? new OriginalTextService(_uow, _uow.TextRepository, _mapper);

    public ITranslationService Translations => _translations ?? new TranslationService(_uow, _uow.TranslationRepository, _mapper);

    public ICourseService Courses => _courses ?? new CourseService(_uow, _uow.CourseRepository, _mapper);

    public IHobbyService Hobbies => _hobbies ?? new HobbyService(_uow, _uow.HobbyRepository, _mapper);

    public ISchoolService Schools => _schools ?? new SchoolService(_uow, _uow.SchoolRepository, _mapper);

    public ISleepService Sleeps => _sleeps ?? new SleepService(_uow, _uow.SleepRepository, _mapper);

    public IUserCourseService UserCourses => _userCourses ?? new UserCourseService(_uow, _uow.UserCourseRepository, _mapper);

    public IUserHobbyService UserHobbies => _userHobbies ?? new UserHobbyService(_uow, _uow.UserHobbyRepository, _mapper);

    public IUserWorkService UserWorkplaces => _userWorkplaces ?? new UserWorkService(_uow, _uow.UserWorkRepository, _mapper);

    public IWorkHoursService WorkHours => _workHours ?? new WorkHoursService(_uow, _uow.WorkHoursRepository, _mapper);

    public IWorkService Workplaces => _workplaces ?? new WorkService(_uow, _uow.WorkRepository, _mapper);
}
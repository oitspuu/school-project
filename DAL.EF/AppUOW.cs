using AutoMapper;
using Base;
using DAL.EF.Repositories;
using DAL.EF.Repositories.Identity;
using DAL.EF.Repositories.Translations;
using DAL.Interfaces;
using DAL.Interfaces.Identity;
using DAL.Interfaces.Translations;

namespace DAL.EF;

public class AppUOW : UnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    private IAppRoleRepository? _appRole;
    private IAppUserRepository? _appUser;
    private IRefreshTokenRepository? _refreshTokenRepository;
    private ITranslationRepository? _translationRepository;
    private ILanguageRepository? _languageRepository;
    private IOriginalTextRepository? _textRepository;
    private ICourseRepository? _courseRepository;
    private IHobbyRepository? _hobbyRepository;
    private ISchoolRepository? _schoolRepository;
    private ISleepRepository? _sleepRepository;
    private IUserCourseRepository? _userCourseRepository;
    private IUserHobbyRepository? _userHobbyRepository;
    private IUserWorkRepository? _userWorkRepository;
    private IWorkHoursRepository? _workHoursRepository;
    private IWorkRepository? _workRepository;
    
    public AppUOW(AppDbContext context, IMapper mapper) : base(context)
    {
        _mapper = mapper;
    }

    public IAppRoleRepository AppRoleRepository => _appRole ?? new AppRoleRepository(Context, _mapper);
    public IAppUserRepository AppUserRepository => _appUser ?? new AppUserRepository(Context, _mapper);
    public IRefreshTokenRepository RefreshTokenRepository =>
        _refreshTokenRepository ?? new RefreshTokenRepository(Context, _mapper);
    public ITranslationRepository TranslationRepository =>
        _translationRepository ?? new TranslationRepository(Context, _mapper);
    public ILanguageRepository LanguageRepository => _languageRepository ?? new LanguageRepository(Context, _mapper);
    public IOriginalTextRepository TextRepository => _textRepository ?? new OriginalTextRepository(Context, _mapper);
    public ICourseRepository CourseRepository => _courseRepository ?? new CourseRepository(Context, _mapper);
    public IHobbyRepository HobbyRepository => _hobbyRepository ?? new HobbyRepository(Context, _mapper);
    public ISchoolRepository SchoolRepository => _schoolRepository ?? new SchoolRepository(Context, _mapper); 
    public ISleepRepository SleepRepository => _sleepRepository ?? new SleepRepository(Context, _mapper);
    public IUserCourseRepository UserCourseRepository =>
        _userCourseRepository ?? new UserCourseRepository(Context, _mapper); 
    public IUserHobbyRepository UserHobbyRepository =>
        _userHobbyRepository ?? new UserHobbyRepository(Context, _mapper);
    public IUserWorkRepository UserWorkRepository => _userWorkRepository ?? new UserWorkRepository(Context, _mapper); 
    public IWorkHoursRepository WorkHoursRepository =>
        _workHoursRepository ?? new WorkHoursRepository(Context, _mapper);
    public IWorkRepository WorkRepository => _workRepository ?? new WorkRepository(Context, _mapper);
    
}
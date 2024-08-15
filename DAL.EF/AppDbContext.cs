using Domain.Identity;
using Domain.Translations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class AppDbContext(DbContextOptions options)
    : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>(options)

{
    public DbSet<Domain.Course> Courses { get; set; } = default!;
    public DbSet<Domain.Hobby> Hobbies { get; set; } = default!;
    public DbSet<Domain.School> Schools { get; set; } = default!;
    public DbSet<Domain.SleepDuration> Sleeps { get; set; } = default!;
    public DbSet<Domain.Work> Workplaces { get; set; } = default!;
    public DbSet<Domain.UserCourse> UserCourses { get; set; } = default!;
    public DbSet<Domain.UserHobby> UserHobbies { get; set; } = default!;
    public DbSet<Domain.UserWork> UserWorkplaces { get; set; } = default!;
    public DbSet<Domain.WorkHours> WorkHours { get; set; } = default!;
    public DbSet<Language> Languages { get; set; } = default!;
    public DbSet<OriginalText> OriginalTexts { get; set; } = default!;
    public DbSet<Translation> Translations { get; set; } = default!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;
}
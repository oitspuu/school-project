using Base.Helpers;
using DAL.EF;
using Domain;
using Domain.Identity;
using Domain.Translations;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Helpers;

public static class SeedData
{
    public static async void Initialize(AppDbContext context, UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {

        if (roleManager.Roles.Any()) return;

        var admin = new AppUser()
        {
            Id = Guid.NewGuid(),
            Email = "admin@test.ee",
            UserName = "admin@test.ee",
            FirstName = "Test",
            LastName = "Admin"
        };

        var user = new AppUser()
        {
            Id = Guid.NewGuid(),
            Email = "user@test.ee",
            UserName = "user@test.ee",
            FirstName = "Test",
            LastName = "User"
        };
        
        await roleManager.CreateAsync(new AppRole()
        {
            Name = Constants.Admin
        });
        await roleManager.CreateAsync(new AppRole()
        {
            Name = Constants.User
        });

        await userManager.CreateAsync(admin, "Parool1!");
        await  userManager.CreateAsync(user, "Kasutaja1!");
        await userManager.AddToRoleAsync(admin, Constants.Admin);
        await userManager.AddToRoleAsync(user, Constants.User);

        var est = new Language()
        {
            Id = Guid.NewGuid(),
            LanguageIdentifier = Constants.Estonian,
            LanguageName = "Estonian"
        };

        var eng = new Language()
        {
            Id = Guid.NewGuid(),
            LanguageIdentifier = Constants.English,
            LanguageName = "English"
        };
        
        context.Languages.AddRange([est, eng]);
   
        var school = new School()
        {
            Id = Guid.NewGuid(),
            Name = "Kool"
        };
        context.Schools.Add(school);

        var work = new Work()
        {
            Id = Guid.NewGuid(),
            WorkName = "Töö",
            LunchBreakDuration = new TimeSpan(0, 30, 0)
        };
        context.Workplaces.Add(work);

        var hobby = new Hobby()
        {
            Id = Guid.NewGuid(),
            HobbyName = "hobi"
        };
        context.Hobbies.Add(hobby);

        await context.SaveChangesAsync();

        var hobbyText = new OriginalText()
        {
            Id = Guid.NewGuid(),
            OriginalText = hobby.HobbyName,
            LanguageId = est.Id
        };
        context.OriginalTexts.Add(hobbyText);
        
        var schoolText = new OriginalText()
        {
            Id = Guid.NewGuid(),
            OriginalText = school.Name,
            LanguageId = est.Id
        };
        context.OriginalTexts.Add(schoolText);

        context.UserHobbies.Add(new UserHobby()
        {
            Id = Guid.NewGuid(),
            HobbyId = hobby.Id,
            AppUserId = user.Id,
            TimeSpent = new TimeSpan(4, 3, 1)
        });

        var course1 = new Course()
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            CourseName = "kursus 1",
            ECTS = 6,
            Teacher = "õps",
            StartDate = DateOnly.Parse("2024-01-29"),
            EndDate = DateOnly.Parse("2024-05-19")
        };
        var course2 = new Course()
        {
            Id = Guid.NewGuid(),
            SchoolId = school.Id,
            CourseName = "kursus 2",
            ECTS = 6,
            StartDate = DateOnly.Parse("2024-01-29"),
            EndDate = DateOnly.Parse("2024-05-19")
        };
        context.Courses.AddRange([course1, course2]);

        var course1Text = new OriginalText()
        {
            Id = Guid.NewGuid(),
            LanguageId = est.Id,
            OriginalText = course1.CourseName
        };
        var course2Text = new OriginalText()
        {
            Id = Guid.NewGuid(),
            LanguageId = est.Id,
            OriginalText = course2.CourseName
        };
        
        context.OriginalTexts.AddRange(course1Text, course2Text);

        var sleep1 = new SleepDuration()
        {
            AppUserId = user.Id,
            Day = DateOnly.Parse("2024-04-20"),
            Start = new TimeOnly(23, 0, 0),
            End = new TimeOnly(6, 30, 0)
        };
        var sleep2 = new SleepDuration()
        {
            AppUserId = user.Id,
            Day = DateOnly.Parse("2024-04-21"),
            Start = new TimeOnly(22, 30, 0),
            End = new TimeOnly(7, 30, 0)
        };
        var sleep3 = new SleepDuration()
        {
            AppUserId = user.Id,
            Day = DateOnly.Parse("2024-03-20"),
            Start = new TimeOnly(0, 0, 0),
            End = new TimeOnly(8, 30, 0)
        };

        context.Sleeps.AddRange([sleep1, sleep2, sleep3]);

        var userWork = new UserWork()
        {
            Id = Guid.NewGuid(),
            AppUserId = user.Id,
            WorkId = work.Id,
            Start = DateOnly.Parse("2010-03-20"),
            End = DateOnly.Parse("2024-03-31")
        };

        await context.SaveChangesAsync();

        var userCourse1 = new UserCourse()
        {
            Id = Guid.NewGuid(),
            AppUserId = user.Id,
            CourseId = course1.Id,
            HomeworkTime = new TimeSpan(2, 1, 39, 2)
        };
        var userCourse2 = new UserCourse()
        {
            Id = Guid.NewGuid(),
            AppUserId = user.Id,
            CourseId = course2.Id,
            HomeworkTime = new TimeSpan(5, 1, 39, 0)
        };
        
        context.UserCourses.AddRange([userCourse1, userCourse2]);

        var workHours1 = new WorkHours()
        {
            Id = Guid.NewGuid(),
            UserWorkId = userWork.Id,
            UserWork = userWork,
            Date = DateOnly.Parse("2024-03-20"),
            EndTime = new TimeOnly(17, 00),
            StartTime = new TimeOnly(08, 00),
            LunchBreak = true
        };
        var workHours2 = new WorkHours()
        {
            Id = Guid.NewGuid(),
            UserWorkId = userWork.Id,
            UserWork = userWork,
            Date = DateOnly.Parse("2024-03-21"),
            EndTime = new TimeOnly(16, 00),
            StartTime = new TimeOnly(08, 00),
            LunchBreak = false
        };
        var workHours3 = new WorkHours()
        {
            Id = Guid.NewGuid(),
            UserWorkId = userWork.Id,
            UserWork = userWork,
            Date = DateOnly.Parse("2024-02-20"),
            EndTime = new TimeOnly(17, 00),
            StartTime = new TimeOnly(08, 00),
            LunchBreak = true
        };
        context.WorkHours.AddRange([workHours1, workHours2, workHours3]);

        context.Translations.Add(new Translation()
        {
            TextId = hobbyText.Id,
            Language = eng,
            Translation = "hobby"
        });
        context.Translations.Add(new Translation()
        {
            TextId = schoolText.Id,
            Language = eng,
            Translation = "school"
        });
        context.Translations.Add(new Translation()
        {
            TextId = course1Text.Id,
            Language = eng,
            Translation = "course 1"
        });
        context.Translations.Add(new Translation()
        {
            TextId = course2Text.Id,
            Language = eng,
            Translation = "course 2"
        });

        await context.SaveChangesAsync();

        hobby.NameTranslations = hobbyText;
        context.Update(hobby);
        
        school.NameTranslations = schoolText;
        context.Update(school);
        
        course1.NameTranslations = course1Text;
        context.Update(course1);
        
        course2.NameTranslations = course2Text;
        context.Update(course2);
        
        await context.SaveChangesAsync();
    }

}
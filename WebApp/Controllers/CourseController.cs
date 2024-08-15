using System;
using System.Globalization;
using System.Threading.Tasks;
using BLL.Interfaces;
using Domain.Identity;
using DTO.v1_0;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

[Authorize]
public class CourseController (IAppBll bll, UserManager<AppUser> userManager) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var culture = CultureInfo.CurrentUICulture.Name.Split("-")[0];
        
        return View(await bll.UserCourses.GetAllUserCoursesAsync(userId, culture));
    }
    
    // GET
    // add time to course
    public async Task<IActionResult> Add(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        if (!await bll.UserCourses.ExistsAsync((Guid)id)) return NotFound();

        var course = new UserCourseAddTime()
        {
            Id = (Guid)id,
        };
        return View(course);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind("Id,TimeSpent")] UserCourseAddTime course)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (!ModelState.IsValid) return View(course);
        
        var success = await bll.UserCourses.AddTime((Guid) userId, course.Id, course.TimeSpent);
        if (!success) return View(course);
        
        await bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("CourseName,HomeworkTime,SchoolName,Teacher,ECTS,StartDate,EndDate")] BLL.DTO.UserCourse userCourse)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (ModelState.IsValid)
        {
            userCourse.Language = CultureInfo.CurrentUICulture.Name.Split("-")[0];
            userCourse.AppUserId = userId;
            
            var added = await bll.UserCourses.AddUserCourse(userCourse);
            if (added == null)
            {
                return NotFound();
            }
            
            await bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View(userCourse);
    }

    // GET
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lang = CultureInfo.CurrentUICulture.Name.Split("-")[0];
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var course = await bll.UserCourses.GetUserCourseAsync((Guid) userId,(Guid) id, lang);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }
    
    // GET
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var lang = CultureInfo.CurrentUICulture.Name.Split("-")[0];
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var userCourse = await bll.UserCourses.GetUserCourseAsync((Guid) userId,(Guid) id, lang);
        if (userCourse == null)
        {
            return NotFound();
        }

        return View(userCourse);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, 
        [Bind("CourseName," +
              "HomeworkTime," +
              "SchoolName," +
              "Teacher," +
              "ECTS," +
              "StartDate," +
              "EndDate," +
              "Id," +
              "CourseId," +
              "SchoolId," +
              "Language," +
              "OriginalCourseTextId," +
              "OriginalSchoolTextId")] BLL.DTO.UserCourse userCourse)
    {
        if (id != userCourse.Id)
        {
            return NotFound();
        }

        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        userCourse.AppUserId = userId;
        
        if (ModelState.IsValid)
        {
            try
            {
                await bll.UserCourses.UpdateUserCourse(userCourse);
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserCourses.ExistsAsync(userCourse.Id))
                {
                    return NotFound();
                }

                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(userCourse);
    }

    // GET
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var lang = CultureInfo.CurrentUICulture.Name.Split("-")[0];
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        var userCourse = await bll.UserCourses.GetUserCourseAsync(userId, (Guid) id, lang);
        if (userCourse == null)
        {
            return NotFound();
        }

        return View(userCourse);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        await bll.UserCourses.RemoveAsync(id, userId);
        await bll.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
}
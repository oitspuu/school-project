using System;
using System.Threading.Tasks;
using BLL.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

[Authorize]
public class WorkController(IAppBll bll, UserManager<AppUser> userManager) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var workplaces = await bll.UserWorkplaces.GetAllUserWorkAsync(userId);
        return View(workplaces);
    }
    
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("WorkName,Start,End,LunchBreakDuration")] BLL.DTO.UserWork userWork)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (ModelState.IsValid)
        {
            userWork.AppUserId = userId;
            
            var added = bll.UserWorkplaces.AddUserWork(userWork);
            if (added == null) return NotFound();

            await bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        return View(userWork);
    }
    
    // GET
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var userWork = await bll.UserWorkplaces.GetUserWorkWithHoursAsync((Guid) userId,(Guid) id);
        if (userWork == null)
        {
            return NotFound();
        }

        return View(userWork);
    }
    
    // GET
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var userWork = await bll.UserWorkplaces.GetUserWorkAsync((Guid) userId,(Guid) id);
        if (userWork == null)
        {
            return NotFound();
        }

        return View(userWork);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, 
        [Bind("WorkName," +
              "Start," +
              "End," +
              "LunchBreakDuration," +
              "Id," +
              "WorkId")] BLL.DTO.UserWork userWork)
    {
        if (id != userWork.Id)
        {
            return NotFound();
        }

        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        userWork.AppUserId = userId;
        
        if (ModelState.IsValid)
        {
            try
            {
                await bll.UserWorkplaces.UpdateUserWork(userWork);
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserCourses.ExistsAsync(userWork.Id))
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
        return View(userWork);
    }
    
    // GET
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        var userWork = await bll.UserWorkplaces.GetUserWorkAsync(userId, (Guid) id);
        if (userWork == null)
        {
            return NotFound();
        }

        return View(userWork);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        await bll.UserWorkplaces.RemoveAsync(id, userId);
        await bll.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Add(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exists = await bll.UserWorkplaces.ExistsAsync((Guid)id);

        if (!exists)
        {
            return NotFound();
        }
        
        var hour = new BLL.DTO.WorkHours()
        {
            UserWorkId = (Guid) id,
        };
        return View(hour);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(
        [Bind("Date,StartTime,EndTime,LunchBreak,UserWorkId")] BLL.DTO.WorkHours hours)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (!ModelState.IsValid) return View(hours);
        
        var success = bll.WorkHours.Add(hours);
        if (success == null) return View(hours);
        
        await bll.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new {id = hours.UserWorkId});
    }
    
    // GET
    public async Task<IActionResult> DeleteDay(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        var workHour = await bll.WorkHours.GetWorkDayAsync(userId, (Guid) id);
        if (workHour == null)
        {
            return NotFound();
        }

        return View(workHour);
    }
    
    // POST
    [HttpPost, ActionName("DeleteDay")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDay(Guid id)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        await bll.WorkHours.RemoveAsync(id, userId);
        await bll.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
}
using System;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

[Authorize]
public class SleepController(IAppBll bll, UserManager<AppUser> userManager) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var list = await bll.Sleeps.GetAllSleepWithTotalAsync(userId);
        return View(list);

    }
    
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Day,Start,End")] SleepDuration sleepDuration)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (ModelState.IsValid)
        {
            sleepDuration.AppUserId = userId;
            bll.Sleeps.Add(sleepDuration);
            await bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(sleepDuration);
    }

    // GET
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sleepDuration = await bll.Sleeps.FirstOrDefaultAsync((Guid) id);
        if (sleepDuration == null)
        {
            return NotFound();
        }
        return View(sleepDuration);
    }
    
    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,Day,Start,End")] SleepDuration sleepDuration)
    {
        if (id != sleepDuration.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
                if (userId == default) return NotFound();
                sleepDuration.AppUserId = userId;

                bll.Sleeps.Update(sleepDuration);
                
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.Sleeps.ExistsAsync(sleepDuration.Id))
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
        return View(sleepDuration);
    }
    
    // GET
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sleepDuration = await bll.Sleeps.FirstOrDefaultAsync((Guid) id);
            
        if (sleepDuration == null)
        {
            return NotFound();
        }
        return View(sleepDuration);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await bll.Sleeps.RemoveAsync(id);
        await bll.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
    
    
}
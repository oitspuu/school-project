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
public class HobbyController(IAppBll bll, UserManager<AppUser> userManager) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var culture = CultureInfo.CurrentUICulture.Name.Split("-")[0];
        
        return View(await bll.UserHobbies.GetAllUserHobbiesAsync(userId, culture));
    }

    // GET
    public async Task<IActionResult> Add(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        if (!await bll.UserHobbies.ExistsAsync((Guid)id)) return NotFound();

        var hobby = new UserHobbyAddTime()
        {
            Id = (Guid)id,
        };
        return View(hobby);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind("Id,TimeSpent")] UserHobbyAddTime hobby)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (!ModelState.IsValid) return View(hobby);
        
        var success = await bll.UserHobbies.AddTime((Guid) userId, hobby.Id, hobby.TimeSpent);
        if (!success) return View(hobby);
        
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
    public async Task<IActionResult> Create([Bind("TimeSpent,HobbyName")] BLL.DTO.UserHobby userHobby)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        if (ModelState.IsValid)
        {
            userHobby.Language = CultureInfo.CurrentUICulture.Name.Split("-")[0];
            userHobby.AppUserId = userId;
            
            await bll.UserHobbies.AddUserHobby(userHobby);
            await bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View(userHobby);
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
        
        var userHobby = await bll.UserHobbies.GetUserHobbyAsync((Guid) userId,(Guid) id, lang);
        if (userHobby == null)
        {
            return NotFound();
        }

        return View(userHobby);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, 
        [Bind("Language,OriginalTextId,HobbyId,HobbyName,TimeSpent,Id")] BLL.DTO.UserHobby userHobby)
    {
        if (id != userHobby.Id)
        {
            return NotFound();
        }

        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        userHobby.AppUserId = userId;
        
        if (ModelState.IsValid)
        {
            try
            {
                await bll.UserHobbies.UpdateUserHobby(userHobby);
                await bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await bll.UserHobbies.ExistsAsync(userHobby.Id))
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
        return View(userHobby);
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

        var userHobby = await bll.UserHobbies.GetUserHobbyAsync(userId, (Guid) id, lang);
        if (userHobby == null)
        {
            return NotFound();
        }

        return View(userHobby);
    }

    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();

        await bll.UserHobbies.RemoveAsync(id, userId);
        await bll.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
}
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using BLL.Interfaces;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
public class HomeController(IAppBll bll, UserManager<AppUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var split = culture.Split("-")[0];
        
        if (!Guid.TryParse(userManager.GetUserId(User), out var userId)) return NotFound();
        if (userId == default) return NotFound();
        
        var appUser = await bll.AppUsers.FirstOrDefaultWithCollectionsAsync(userId, split);
        return View(appUser);

    }
    
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions()
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            }
        );
        return LocalRedirect(returnUrl);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
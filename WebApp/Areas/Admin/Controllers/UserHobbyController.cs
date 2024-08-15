using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.EF;
using Domain;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserHobbyController : Controller
    {
        private readonly AppDbContext _context;

        public UserHobbyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserHobby
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserHobbies.Include(u => u.AppUser).Include(u => u.Hobby);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/UserHobby/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userHobby = await _context.UserHobbies
                .Include(u => u.AppUser)
                .Include(u => u.Hobby)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userHobby == null)
            {
                return NotFound();
            }

            return View(userHobby);
        }

        // GET: Admin/UserHobby/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            ViewData["HobbyId"] = new SelectList(_context.Hobbies, "Id", "HobbyName");
            return View();
        }

        // POST: Admin/UserHobby/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,HobbyId,TimeSpent,Id")] UserHobby userHobby)
        {
            if (ModelState.IsValid)
            {
                userHobby.Id = Guid.NewGuid();
                _context.Add(userHobby);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userHobby.AppUserId);
            ViewData["HobbyId"] = new SelectList(_context.Hobbies, "Id", "HobbyName", userHobby.HobbyId);
            return View(userHobby);
        }

        // GET: Admin/UserHobby/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userHobby = await _context.UserHobbies.FindAsync(id);
            if (userHobby == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userHobby.AppUserId);
            ViewData["HobbyId"] = new SelectList(_context.Hobbies, "Id", "HobbyName", userHobby.HobbyId);
            return View(userHobby);
        }

        // POST: Admin/UserHobby/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,HobbyId,TimeSpent,Id")] UserHobby userHobby)
        {
            if (id != userHobby.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userHobby);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserHobbyExists(userHobby.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userHobby.AppUserId);
            ViewData["HobbyId"] = new SelectList(_context.Hobbies, "Id", "HobbyName", userHobby.HobbyId);
            return View(userHobby);
        }

        // GET: Admin/UserHobby/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userHobby = await _context.UserHobbies
                .Include(u => u.AppUser)
                .Include(u => u.Hobby)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userHobby == null)
            {
                return NotFound();
            }

            return View(userHobby);
        }

        // POST: Admin/UserHobby/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userHobby = await _context.UserHobbies.FindAsync(id);
            if (userHobby != null)
            {
                _context.UserHobbies.Remove(userHobby);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserHobbyExists(Guid id)
        {
            return _context.UserHobbies.Any(e => e.Id == id);
        }
    }
}

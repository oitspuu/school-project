using System;
using System.Collections.Generic;
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
    public class UserWorkController : Controller
    {
        private readonly AppDbContext _context;

        public UserWorkController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserWork
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserWorkplaces.Include(u => u.AppUser).Include(u => u.Work);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/UserWork/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWork = await _context.UserWorkplaces
                .Include(u => u.AppUser)
                .Include(u => u.Work)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userWork == null)
            {
                return NotFound();
            }

            return View(userWork);
        }

        // GET: Admin/UserWork/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            ViewData["WorkId"] = new SelectList(_context.Workplaces, "Id", "WorkName");
            return View();
        }

        // POST: Admin/UserWork/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,WorkId,FullTime,Start,End,Id")] UserWork userWork)
        {
            if (ModelState.IsValid)
            {
                userWork.Id = Guid.NewGuid();
                _context.Add(userWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userWork.AppUserId);
            ViewData["WorkId"] = new SelectList(_context.Workplaces, "Id", "WorkName", userWork.WorkId);
            return View(userWork);
        }

        // GET: Admin/UserWork/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWork = await _context.UserWorkplaces.FindAsync(id);
            if (userWork == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userWork.AppUserId);
            ViewData["WorkId"] = new SelectList(_context.Workplaces, "Id", "WorkName", userWork.WorkId);
            return View(userWork);
        }

        // POST: Admin/UserWork/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,WorkId,FullTime,Start,End,Id")] UserWork userWork)
        {
            if (id != userWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserWorkExists(userWork.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userWork.AppUserId);
            ViewData["WorkId"] = new SelectList(_context.Workplaces, "Id", "WorkName", userWork.WorkId);
            return View(userWork);
        }

        // GET: Admin/UserWork/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userWork = await _context.UserWorkplaces
                .Include(u => u.AppUser)
                .Include(u => u.Work)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userWork == null)
            {
                return NotFound();
            }

            return View(userWork);
        }

        // POST: Admin/UserWork/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userWork = await _context.UserWorkplaces.FindAsync(id);
            if (userWork != null)
            {
                _context.UserWorkplaces.Remove(userWork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserWorkExists(Guid id)
        {
            return _context.UserWorkplaces.Any(e => e.Id == id);
        }
    }
}

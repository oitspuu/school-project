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
    public class SleepDurationController(AppDbContext context) : Controller
    {
        // GET: Admin/SleepDuration
        public async Task<IActionResult> Index()
        {
            var appDbContext = context.Sleeps.Include(s => s.AppUser);
            var list = await appDbContext.ToListAsync();
            return View(list);
        }

        // GET: Admin/SleepDuration/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sleepDuration = await context.Sleeps
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sleepDuration == null)
            {
                return NotFound();
            }
            return View(sleepDuration);
        }

        // GET: Admin/SleepDuration/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(context.Users, "Id", "FirstName");
            return View();
        }

        // POST: Admin/SleepDuration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,AppUserId,Day,Start,End")] SleepDuration sleepDuration)
        {
            if (ModelState.IsValid)
            {
                context.Add(sleepDuration);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(context.Users, "Id", "FirstName", sleepDuration.AppUserId);
            return View(sleepDuration);
        }

        // GET: Admin/SleepDuration/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sleepDuration = await context.Sleeps.FindAsync(id);
            if (sleepDuration == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(context.Users, "Id", "FirstName", sleepDuration.AppUserId);
            return View(sleepDuration);
        }

        // POST: Admin/SleepDuration/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AppUserId,Day,Start,End")] SleepDuration sleepDuration)
        {
            if (id != sleepDuration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(sleepDuration);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SleepDurationExists(sleepDuration.Id))
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
            ViewData["AppUserId"] = new SelectList(context.Users, "Id", "FirstName", sleepDuration.AppUserId);
            return View(sleepDuration);
        }

        // GET: Admin/SleepDuration/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sleepDuration = await context.Sleeps
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sleepDuration == null)
            {
                return NotFound();
            }
            return View(sleepDuration);
        }

        // POST: Admin/SleepDuration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sleepDuration = await context.Sleeps.FindAsync(id);
            if (sleepDuration != null)
            {
                context.Sleeps.Remove(sleepDuration);
            }

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SleepDurationExists(Guid id)
        {
            return context.Sleeps.Any(e => e.Id == id);
        }
    }
}

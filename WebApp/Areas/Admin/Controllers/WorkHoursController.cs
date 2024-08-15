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
    public class WorkHoursController : Controller
    {
        private readonly AppDbContext _context;

        public WorkHoursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/WorkHours
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.WorkHours.Include(w => w.UserWork);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/WorkHours/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workHours = await _context.WorkHours
                .Include(w => w.UserWork)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workHours == null)
            {
                return NotFound();
            }

            return View(workHours);
        }

        // GET: Admin/WorkHours/Create
        public IActionResult Create()
        {
            ViewData["UserWorkId"] = new SelectList(_context.UserWorkplaces, "Id", "Id");
            return View();
        }

        // POST: Admin/WorkHours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserWorkId,Date,StartTime,EndTime,LunchBreak")] WorkHours workHours)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workHours);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserWorkId"] = new SelectList(_context.UserWorkplaces, "Id", "Id", workHours.UserWorkId);
            return View(workHours);
        }

        // GET: Admin/WorkHours/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workHours = await _context.WorkHours.FindAsync(id);
            if (workHours == null)
            {
                return NotFound();
            }
            ViewData["UserWorkId"] = new SelectList(_context.UserWorkplaces, "Id", "Id", workHours.UserWorkId);
            return View(workHours);
        }

        // POST: Admin/WorkHours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserWorkId,Date,StartTime,EndTime,LunchBreak")] WorkHours workHours)
        {
            if (id != workHours.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workHours);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkHoursExists(workHours.Id))
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
            ViewData["UserWorkId"] = new SelectList(_context.UserWorkplaces, "Id", "Id", workHours.UserWorkId);
            return View(workHours);
        }

        // GET: Admin/WorkHours/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workHours = await _context.WorkHours
                .Include(w => w.UserWork)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workHours == null)
            {
                return NotFound();
            }

            return View(workHours);
        }

        // POST: Admin/WorkHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workHours = await _context.WorkHours.FindAsync(id);
            if (workHours != null)
            {
                _context.WorkHours.Remove(workHours);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkHoursExists(Guid id)
        {
            return _context.WorkHours.Any(e => e.Id == id);
        }
    }
}

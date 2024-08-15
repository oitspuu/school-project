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
    public class UserCourseController : Controller
    {
        private readonly AppDbContext _context;

        public UserCourseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/UserCourse
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserCourses.Include(u => u.AppUser).Include(u => u.Course);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/UserCourse/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourses
                .Include(u => u.AppUser)
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // GET: Admin/UserCourse/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName");
            return View();
        }

        // POST: Admin/UserCourse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,AppUserId,HomeworkTime,Id")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                userCourse.Id = Guid.NewGuid();
                _context.Add(userCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userCourse.AppUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", userCourse.CourseId);
            return View(userCourse);
        }

        // GET: Admin/UserCourse/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourses.FindAsync(id);
            if (userCourse == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userCourse.AppUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", userCourse.CourseId);
            return View(userCourse);
        }

        // POST: Admin/UserCourse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourseId,AppUserId,HomeworkTime,Id")] UserCourse userCourse)
        {
            if (id != userCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCourseExists(userCourse.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "FirstName", userCourse.AppUserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", userCourse.CourseId);
            return View(userCourse);
        }

        // GET: Admin/UserCourse/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourses
                .Include(u => u.AppUser)
                .Include(u => u.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // POST: Admin/UserCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userCourse = await _context.UserCourses.FindAsync(id);
            if (userCourse != null)
            {
                _context.UserCourses.Remove(userCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCourseExists(Guid id)
        {
            return _context.UserCourses.Any(e => e.Id == id);
        }
    }
}

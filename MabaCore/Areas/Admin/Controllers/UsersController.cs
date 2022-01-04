using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MabaCore.Data;
using MabaCore.Data.ViewModels;

namespace MabaCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Select(m=> new VMUser { 
                Email = m.Email,
                FirstName = m.FirstName,
                LastName =m.LastName,
                Location = m.Location,
                Phone = m.PhoneNumber
            }).ToListAsync());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vMUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vMUser == null)
            {
                return NotFound();
            }

            return View(vMUser);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,FirstName,LastName,Phone,Location")] VMUser vMUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vMUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vMUser);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vMUser = await _context.Users.FindAsync(id);
            if (vMUser == null)
            {
                return NotFound();
            }
            return View(vMUser);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,FirstName,LastName,Phone,Location")] VMUser vMUser)
        {
            if (id != vMUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vMUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VMUserExists(vMUser.Id))
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
            return View(vMUser);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vMUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vMUser == null)
            {
                return NotFound();
            }

            return View(vMUser);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vMUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(vMUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VMUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

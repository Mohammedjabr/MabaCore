using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MabaCore.Data;
using MabaCore.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using MabaCore.Data.Models;

namespace MabaCore.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            var model = _context.Users.Where(m => m.Id == _userManager.GetUserId(User)).
                                        Select(m => new VMUser
                                        {
                                        Email = m.Email,
                                        FirstName = m.FirstName,
                                        LastName = m.LastName,
                                        Location = m.Location,
                                        Phone = m.PhoneNumber
                                        }).SingleOrDefault();
            //var user = await _context.Users.FindAsync(_userManager.GetUserId(User));
            //var model = new VMUser();
            //model.Email = user.Email;
            //model.FirstName = user.FirstName;
            //model.LastName = user.LastName;
            //model.Location = user.Location;
            //model.Phone = user.PhoneNumber;
            return View(model);
        }

        //// GET: Profile/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vMUser = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (vMUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vMUser);
        //}

        //// GET: Profile/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Profile/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Email,FirstName,LastName,Phone,Location")] VMUser vMUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(vMUser);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(vMUser);
        //}

        //// GET: Profile/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vMUser = await _context.Users.FindAsync(id);
        //    if (vMUser == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(vMUser);
        //}

        // POST: Profile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VMUser vMUser)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var model = _context.Users.Find(_userManager.GetUserId(User));
                    model.Email = vMUser.Email;
                    model.UserName = vMUser.Email;
                    model.FirstName = vMUser.FirstName;
                    model.LastName = vMUser.LastName;
                    model.Location = vMUser.Location;
                    model.PhoneNumber = vMUser.Phone;
                    _context.Users.Update(model);
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

        //// GET: Profile/Delete/5
        //public async Task<IActionResult> Delete(string? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var vMUser = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (vMUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vMUser);
        //}

        //// POST: Profile/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var vMUser = await _context.Users.FindAsync(id);
        //    _context.Users.Remove(vMUser);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool VMUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}

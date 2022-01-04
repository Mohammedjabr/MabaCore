using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MabaCore.Data;
using MabaCore.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace MabaCore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProductsUsers.Where(p => p.UserId == _userManager.GetUserId(User)).Include(p => p.Product).Include(p => p.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productUser = await _context.ProductsUsers
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productUser == null)
            {
                return NotFound();
            }

            return View(productUser);
        }

        // GET: Cart/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserId")] ProductUser productUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productUser.ProductId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", productUser.UserId);
            return View(productUser);
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productUser = await _context.ProductsUsers.FindAsync(id);
            if (productUser == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productUser.ProductId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", productUser.UserId);
            return View(productUser);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId")] ProductUser productUser)
        {
            if (id != productUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductUserExists(productUser.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productUser.ProductId);
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", productUser.UserId);
            return View(productUser);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productUser = await _context.ProductsUsers
                .Include(p => p.Product)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productUser == null)
            {
                return NotFound();
            }

            return View(productUser);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productUser = await _context.ProductsUsers.FindAsync(id);
            _context.ProductsUsers.Remove(productUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductUserExists(int id)
        {
            return _context.ProductsUsers.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult AddToCart(int id)
        {
            var userId = _userManager.GetUserId(User);
            if (_context.ProductsUsers.Any(m => m.UserId == userId && m.ProductId == id))
            {
                var product = _context.ProductsUsers.Where(m => m.UserId == userId && m.ProductId == id).FirstOrDefault();
                product.Count = ++product.Count;

            }
            else
            {
                var cart = new ProductUser
                {
                    ProductId = id,
                    UserId = userId,
                    Count = 1
                };
                _context.ProductsUsers.Add(cart);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
    }
}

using MabaCore.Data;
using MabaCore.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabaCore.Web.ViewComponents
{
    public class CartCountViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartCountViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.ProductsUsers.Where(m=>m.UserId == _userManager.GetUserId(HttpContext.User)).Count());
        }
    }
}

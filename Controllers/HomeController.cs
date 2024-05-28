using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web1.Models;
using Web1.Models.DBModels;

namespace Web1.Controllers
{ 
    public class HomeController : Controller
    {
        public readonly MyDbContext Context;
        public HomeController(MyDbContext context)
        {
            Context = context;
        }
        public void IsRole()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userRole = null;
            if (userId != null)
            {
                var user = Context.User.FirstOrDefault(u => u.UserID == Convert.ToInt32(userId));
                if (user != null)
                {
                    var userAccount = Context.UserAccount.FirstOrDefault(u => u.UserLogin == user.UserAccount);
                    if (userAccount != null)
                    {
                        userRole = userAccount.UserRole;
                    }
                }    
            }
            ViewBag.UserRole = userRole;
        }

        public IActionResult Index()
        {
            IsRole();
            if (User.Identity!.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Authorize");
            }

        }
        [HttpGet]
        public async Task<IActionResult> Flowers()
        {
            var userLogin = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = await Context.UserAccount
                                .Where(a => a.UserLogin == userLogin)
                                .Select(a => a.UserRole)
                                .FirstOrDefaultAsync();
            var products = await Context.Product.ToListAsync();
            ViewBag.Categories = await Context.Product
                                               .Select(p => p.ProductCategory)
                                               .Distinct()
                                               .ToListAsync();
            if (userRole == 1)
            {
                return View("About");
            }
            return View("Flowers", products);
        }
        public async Task<IActionResult> FilterFlowers(string category, string sortBy)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Flowers");
            }

            var query = Context.Product.Where(p => p.ProductCategory.ToString() == category);

            // Добавляем сортировку по цене
            switch (sortBy)
            {
                case "asc":
                    query = query.OrderBy(p => p.ProductPrice);
                    break;
                case "desc":
                    query = query.OrderByDescending(p => p.ProductPrice);
                    break;
                // По умолчанию сортируем по возрастанию
                default:
                    query = query.OrderBy(p => p.ProductPrice);
                    break;
            }

            var products = await query.ToListAsync();

            ViewBag.Categories = await Context.Product
                                               .Select(p => p.ProductCategory)
                                               .Distinct()
                                               .ToListAsync();

            return View("Flowers", products);
        }

        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Services()
        {
            var service = await Context.Service.ToListAsync();
            return View(service);
        }

    }
}

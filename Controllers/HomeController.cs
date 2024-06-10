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
        public IActionResult Index()
        {
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
            var products = await Context.Product.ToListAsync();
            ViewBag.Categories = await Context.Product
                                               .Select(p => p.ProductCategory)
                                               .Distinct()
                                               .ToListAsync();
            
            return View("Flowers", products);
        }
        public async Task<IActionResult> FilterFlowers(string category, string sortBy)
        {
            if (string.IsNullOrEmpty(category))
            {
                return RedirectToAction("Flowers");
            }

            var query = Context.Product.Where(p => p.ProductCategory == category);

            switch (sortBy)
            {
                case "asc":
                    query = query.OrderBy(p => p.ProductPrice);
                    break;
                case "desc":
                    query = query.OrderByDescending(p => p.ProductPrice);
                    break;
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
        public async Task<IActionResult> Orders()
        {
            var orders = await Context.Order
                .Include(u => u.User)
                .ToListAsync();

            var viewModel = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                var orderViewModel = new OrderViewModel
                {
                    OrderID = order.OrderID,
                    UserName = order.User.UserName,
                    UserPhone = order.User.UserPhone,
                    OrderDate = order.OrderDate,
                    OrderProductsList = await Context.OrderProducts.Where(sl => sl.OrderID == order.OrderID).ToListAsync(),
                    OrderServicesList = await Context.OrderServices.Where(sl => sl.OrderID == order.OrderID).ToListAsync()

                };

                viewModel.Add(orderViewModel);
            }

            return View(viewModel);
        }
        //public async Task FindOrder()
        //{
        //    var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
        //    var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

        //    if (user != null)
        //    {
        //        var userOrders = await Context.Order
        //                                      .Where(o => o.UserID == user.UserID)
        //                                      .ToListAsync();
        //        ViewBag.UserOrders = userOrders.Count;
        //    }
        //    else
        //    {
        //        ViewBag.UserOrders = 0;
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productName, double unitPrice)
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

            var order = new Order
            {
                UserID = user.UserID,
                DeliveryID = 1, 
                OrderDate = DateTime.Now
            };
            try
            {
                Context.Order.Add(order);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error in order " + ex.Message);
            }
            var model = new OrderProducts
            {
                ProductName = productName,
                ProductCount = 1,
                OrderID = order.OrderID,
            };
            if (ModelState.IsValid)
            {
                try
                {
                    Context.OrderProducts.Add(model);
                    Context.SaveChanges();
                    //await FindOrder();
                    return RedirectToAction("Order", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "error in orderProducts " + ex.Message);
                }
            }
            return View();
        }

    }
}

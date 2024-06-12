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
        .Where(o => o.IsCompleted == true) // Filter completed orders
        .OrderBy(o => o.OrderDate) // Sort orders by date ascending
        .ToListAsync();

            var viewModel = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                if (order.IsCompleted == true)
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
            }
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(string productName, double unitPrice)
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);
            var order = await Context.Order
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.UserID == user.UserID && !o.IsCompleted);
            if (order == null)
            {
                order = new Order
                {
                    UserID = user.UserID,
                    DeliveryID = 1,
                    OrderDate = DateTime.Now,
                    IsCompleted = false
                };

                try
                {
                    Context.Order.Add(order);
                    Context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "error in order " + ex.Message);
                    return View();
                }
            }

            if (order != null && order.OrderProducts != null)
            {
                var existingProduct = order.OrderProducts.FirstOrDefault(op => op.ProductName == productName);
                if (existingProduct != null)
                {
                    existingProduct.ProductCount += 1;
                }
                else
                {
                    var orderProduct = new OrderProducts
                    {
                        ProductName = productName,
                        ProductCount = 1,
                        OrderID = order.OrderID,
                    };

                    order.OrderProducts.Add(orderProduct);
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        Context.SaveChanges();
                        ViewBag.ProductCount = order.OrderProducts.Sum(op => op.ProductCount);
                        return RedirectToAction("Order", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "error in orderProducts " + ex.Message);
                    }
                }
            }
            
            return View();
        }
        public async Task<IActionResult> Order()
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

            var orders = await Context.Order
                .Include(o => o.OrderProducts)
                .Where(o => o.UserID == user.UserID && !o.IsCompleted)
                .ToListAsync();

            var orderProducts = orders.SelectMany(o => o.OrderProducts)
                                  .Select(op => new
                                  {
                                      Product = new Product
                                      {
                                          ProductName = op.ProductName,
                                          ProductPrice = Context.Product.FirstOrDefault(p => p.ProductName == op.ProductName).ProductPrice
                                      },
                                      ProductCount = op.ProductCount
                                  })
                                  .ToList();

            ViewBag.ProductCountList = orderProducts.ToDictionary(op => op.Product.ProductName, p => p.ProductCount);
            var products = orderProducts.Select(op => op.Product).ToList();
            decimal totalAmount = orderProducts.Sum(op => op.Product.ProductPrice * op.ProductCount);
            ViewBag.TotalAmount = totalAmount;

            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(string productName)
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

            var order = await Context.Order
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.UserID == user.UserID && !o.IsCompleted);

            if (order != null && order.OrderProducts != null)
            {
                var productToRemove = order.OrderProducts.FirstOrDefault(op => op.ProductName == productName);
                if (productToRemove != null)
                {
                    order.OrderProducts.Remove(productToRemove);
                    try
                    {
                        await Context.SaveChangesAsync();
                        return RedirectToAction("Order", "Home");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "error in removing product " + ex.Message);
                    }
                }
            }

            return RedirectToAction("Order", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

            var order = await Context.Order
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.UserID == user.UserID && !o.IsCompleted);
            if (order != null)
            {
                order.IsCompleted = true;
                order.OrderDate = DateTime.Now;
                Context.SaveChanges();

                return RedirectToAction("Payment");
            }
            return RedirectToAction("Order");
        }
        [HttpPost]
        public async Task<IActionResult> IncreaseProductCount(string productName)
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

            var order = await Context.Order
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.UserID == user.UserID && !o.IsCompleted);

            if (order != null && order.OrderProducts != null)
            {
                var existingProduct = order.OrderProducts.FirstOrDefault(op => op.ProductName == productName);
                if (existingProduct != null)
                {
                    existingProduct.ProductCount += 1;
                    await Context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Order", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> DecreaseProductCount(string productName)
        {
            var userName = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await Context.User.FirstOrDefaultAsync(u => u.UserLogin == userName);

            var order = await Context.Order
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.UserID == user.UserID && !o.IsCompleted);

            if (order != null && order.OrderProducts != null)
            {
                var existingProduct = order.OrderProducts.FirstOrDefault(op => op.ProductName == productName);
                if (existingProduct != null && existingProduct.ProductCount > 1)
                {
                    existingProduct.ProductCount -= 1;
                    await Context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Order", "Home");
        }
        public IActionResult Payment()
        {
            return View();
        }
    }
}

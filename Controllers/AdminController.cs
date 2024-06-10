using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Web1.Models;
using Web1.Models.DBModels;
using static System.Net.Mime.MediaTypeNames;

namespace Web1.Controllers
{
    public class AdminController : Controller
    {
        public readonly MyDbContext Context;
        public AdminController(MyDbContext context)
        {
            Context = context;
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> AddProduct(IFormCollection form, IFormFile image)
        {
            int maxProductNum = await Context.Product.MaxAsync(p => p.ProductNum);
            int newProductNum = maxProductNum + 1;

            var newProduct = new Product
            {
                ProductName = form["name"],
                ProductCategory = form["category"],
                ProductPrice = Convert.ToDecimal(form["price"]),
                ProductDescription = form["description"],
                ProductNum = newProductNum,
                ProductProvider = Convert.ToInt32(form["providers"])
            };
            try
            {
                if (image != null && image.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProductImg");
                    var filePath = Path.Combine(uploads, newProductNum.ToString().Replace(" ", "") + ".jpg");

                    // Сохранение файла
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }

                Context.Product.Add(newProduct);
                Context.SaveChanges();
                return RedirectToAction("Flowers", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Произошла ошибка при добавлении товара");
            }

            return View(newProduct);
        }
        public IActionResult EditProduct(int productNum)
        {
            var product = Context.Product.FirstOrDefault(p => p.ProductNum == productNum);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            var existingProduct = await Context.Product.FirstOrDefaultAsync(p => p.ProductNum == product.ProductNum);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductCategory = product.ProductCategory;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductDescription = product.ProductDescription;
           
            Context.Product.Update(existingProduct);
            await Context.SaveChangesAsync();

            return RedirectToAction("Flowers", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string productName)
        {
            var product = await Context.Product.FindAsync(productName);
            if (product == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProductImg", product.ProductNum.ToString().Replace(" ", "") + ".jpg");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            Context.Product.Remove(product);
            await Context.SaveChangesAsync();

            return RedirectToAction("Flowers", "Home");
        }
        public IActionResult AddService()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "1")]
        public IActionResult AddService(IFormCollection form)
        { 
            var newService = new Service
            {
                ServiceName = form["name"],
                ServicePrice = Convert.ToDecimal(form["price"]),
                ServiceDescription = form["description"]
            };
            try
            {
                Context.Service.Add(newService);
                Context.SaveChanges();
                return RedirectToAction("Flowers", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Произошла ошибка при добавлении товара");
            }

            return View(newService);
        }
        public async Task<IActionResult> Team()
        {
            var users = await Context.User
                                 .Include(u => u.Role)
                                 .Where(u => u.RoleID != 3)
                                 .ToListAsync();
            ViewBag.Roles = new SelectList(await Context.UserRole
                                                   .Where(r => r.RoleID != 3)
                                                   .ToListAsync(),
                                       "RoleID", "RoleName");
            return View(users);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(IFormCollection form)
        {
            var newUser = new User
            {
                UserName = form["UserName"],
                UserLogin = form["UserLogin"],
                UserPhone = form["UserPhone"],
                UserPassword = HashPassword(form["UserPassword"]),
                RoleID = Convert.ToInt32(form["RoleID"])
            };

            Context.User.Add(newUser);
            await Context.SaveChangesAsync();

            return RedirectToAction("Team");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await Context.User.FindAsync(userId);
            if (user != null)
            {
                Context.User.Remove(user);
                await Context.SaveChangesAsync();
            }

            return RedirectToAction("Team");
        }
        public async Task<IActionResult> Shipments()
        {
            var shipments = await Context.Shipment
                    .Include(s => s.Provider)
                    .Include(s => s.Warehouse)
                    .ToListAsync();

            var viewModel = new List<ShipmentViewModel>();

            foreach (var shipment in shipments)
            {
                var shipmentViewModel = new ShipmentViewModel
                {
                    ShipmentID = shipment.ShipmentID,
                    ProviderName = shipment.Provider.ProviderName,
                    WarehouseName = shipment.Warehouse.WarehouseName,
                    ShipmentDate = shipment.ShipmentDate,
                    ShipmentLists = await Context.ShipmentList.Where(sl => sl.ShipmentID == shipment.ShipmentID).ToListAsync()
                };

                viewModel.Add(shipmentViewModel);
            }

            return View(viewModel);
        }

    }
}

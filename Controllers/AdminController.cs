using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web1.Models;
using Web1.Models.DBModels;

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
        public async Task<IActionResult> AddProduct(IFormCollection form)
        {
            int maxProductNum = await Context.Product.MaxAsync(p => p.ProductNum);
            int newProductNum = maxProductNum + 1;

            var newProduct = new Product
            {
                ProductName = form["name"],
                ProductCategory = Context.Category.FirstOrDefault(q => q.CategoryName == form["category"]),
                ProductPrice = Convert.ToDecimal(form["price"]),
                ProductDescription = form["description"],
                ProductNum = newProductNum,
                ProductProvider = Convert.ToInt32(form["providers"])
            };
            try
            {
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
 
    }
}

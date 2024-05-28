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
            Context.Category.Load();
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(IFormCollection form)
        {
            //int maxProductNum = await Context.Product.MaxAsync(p => p.ProductNum);
            int maxProductNum = 1;
            int newProductNum = maxProductNum + 1;
            string fl = form["category"].ToString();
            Category categor = await Context.Category.FirstOrDefaultAsync(q => q.CategoryName == fl);
            if(categor == null)
            {
                throw new Exception("is null");
            }
            var newProduct = new Product
            {
                ProductName = form["name"],
                ProductCategory = categor,
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

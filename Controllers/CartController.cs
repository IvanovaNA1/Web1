using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web1.Models;
using Web1.Models.DBModels;

namespace Web1.Controllers
{
    public class CartController : Controller
    {
        public readonly MyDbContext Context;
        public CartController(MyDbContext context)
        {
            Context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int count)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var order = await Context.Order
                .FirstOrDefaultAsync(o => o.UserID.ToString() == userId);

            if (order == null)
            {
                order = new Order
                {
                    UserID = Convert.ToInt32(userId)
                };

                Context.Order.Add(order);
                await Context.SaveChangesAsync();
            }

            var orderProduct = new OrderProducts
            {
                OrderID = order.OrderID,
                ProductName = "Product Name",
                ProductCount = count
            };

            Context.OrderProduct.Add(orderProduct);
            await Context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var orders = await Context.Order
                .Where(o => o.UserID.ToString() == userId)
            .ToListAsync();

            var orderIds = orders.Select(o => o.OrderID).ToList();

            var orderProducts = await Context.OrderProduct
            .Where(op => orderIds.Contains(op.OrderID))
                .ToListAsync();

            var orderServices = await Context.OrderService
                .Where(os => orderIds.Contains(os.OrderID))
                .ToListAsync();

            //var viewModel = new CartViewModel
            //{
            //    Orders = orders,
            //    OrderProducts = orderProducts,
            //    OrderServices = orderServices
            //};

            return View();
        }
    }


}


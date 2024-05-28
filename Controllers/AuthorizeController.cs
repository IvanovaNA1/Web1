using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Web1.Models;
using Web1.Models.DBModels;

namespace Web1.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly MyDbContext myContext;
        public AuthorizeController(MyDbContext _context)
        {
            myContext = _context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserAccount model)
        {
            if (ModelState.IsValid)
            {
                var user = await myContext.UserAccount.FirstOrDefaultAsync(u => u.UserLogin == model.UserLogin);
                if (user != null)
                {
                    var result = await myContext.UserAccount.FirstOrDefaultAsync(p => p.UserPassword == model.UserPassword);
                    if (result != null)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, model.UserLogin)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserAccount model)
        {
            if (ModelState.IsValid)
            {
                var existAccount = myContext.UserAccount.FirstOrDefault(u => u.UserLogin == model.UserLogin);
                if(existAccount != null)
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким логином уже существует");
                    return View(model);
                }
                var userAccount = new UserAccount
                {
                    UserLogin = model.UserLogin,
                    UserPassword = model.UserPassword,
                    UserRole = 3
                };
                myContext.UserAccount.Add(userAccount);
                myContext.SaveChanges();

                return RedirectToAction("Login", "Authorize");
            }
            return View("Register", model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

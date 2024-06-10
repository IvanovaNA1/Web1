using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await myContext.User.FirstOrDefaultAsync(u => u.UserLogin == model.UserLogin);
                if (user != null)
                {   
                    var hashedPassword = HashPassword(model.UserPassword);
                    if (user.UserPassword == hashedPassword)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserLogin),
                            new Claim(ClaimsIdentity.DefaultRoleClaimType,user.RoleID.ToString())
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
        public IActionResult Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existAccount = myContext.User.FirstOrDefault(u => u.UserLogin == model.UserLogin);
                if (existAccount != null)
                {
                    ModelState.AddModelError(string.Empty, "Пользователь с таким логином уже существует");
                    return View(model);
                }
                var user = new User
                {
                    UserName = model.UserName,
                    UserPhone = model.UserPhone,
                    UserLogin = model.UserLogin,
                    UserPassword = HashPassword(model.UserPassword),
                    RoleID = 3
                };
                myContext.User.Add(user);
                myContext.SaveChanges();

                return RedirectToAction("Login", "Authorize");
            }
            return View("Register", model);
        }
        public async Task<IActionResult> Profile()
        {
            var userLogin = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            if (userLogin == null)
            {
                return RedirectToAction("Login", "Authorize");
            }

            var user = await myContext.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserLogin == userLogin);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(User model)
        {
            var userLogin = User.FindFirstValue(ClaimsIdentity.DefaultNameClaimType);
            var user = await myContext.User.FirstOrDefaultAsync(u => u.UserLogin == userLogin);

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.UserPhone = model.UserPhone;
            //user.UserPassword = model.UserPassword;
            if (!string.IsNullOrEmpty(model.UserPassword))
            {
                user.UserPassword = HashPassword(model.UserPassword);
            }

            myContext.User.Update(user);
            await myContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Authorize");
        }
    }
}

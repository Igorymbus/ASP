using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kovukov.Models;
using System.ComponentModel.DataAnnotations;

namespace Yakshin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name; // Получаем email пользователя
            var user = await _appDbContext.Customers.FirstOrDefaultAsync(u => u.email == userEmail);

            if (user != null)
            {
                ViewBag.Greeting = $"Здравствуйте, {user.first_name}. {user.role_name}";
            }
            return View();
        }

        public async Task<IActionResult> Catalog()
        {
            var product = await _appDbContext.Products.ToListAsync();
            return View(product);
        }

        public IActionResult SignIn()
        {
            if (HttpContext.Session.Keys.Contains("AutihatUsers"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Customers user = await _appDbContext.Customers.FirstOrDefaultAsync(u => u.email == model.email && u.passwords == model.passwords);
                if (user != null)
                {
                    
                    await Authenticate(model.email);
                    return RedirectToAction("Prof", new {email = model.email }); 
                }

                ModelState.AddModelError("", "Некорректные логин или пароль");
            }
            return View(model);
        }

      

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AuthUser");
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(Customers person)
        {
            if (ModelState.IsValid)
            {
                // Проверка на уникальность имени пользователя 
                var existingUser = await _appDbContext.Customers.FirstOrDefaultAsync(u => u.email == person.email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Пользователь с таким именем уже существует.");
                    return View(person);
                }

                _appDbContext.Customers.Add(person);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("SignIn");
            }
            return View(person);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<IActionResult> Prof(string email)
        {
            var users = await _appDbContext.Customers.ToListAsync();

            ViewBag.email = email;

            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}

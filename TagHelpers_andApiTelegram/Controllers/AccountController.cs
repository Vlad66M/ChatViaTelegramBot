using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using TagHelpers_andApiTelegram.ViewModel;
using TagHelpers_andApiTelegram.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace TagHelpers_andApiTelegram.Controllers
{
    public class AccountController : Controller
    {
        List<Models.User> users;
        List<Role> roles;
        public AccountController()
        {

            /*var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
            using (var dbContext = new ApplicationContext(options))
            {
                Role role0 = new Role() { Name = "admin" };
                Role role1 = new Role() { Name = "user" };
                Models.User user0 = new Models.User("email1@re.ru", "12345", 1, role0);
                Models.User user1 = new Models.User("email2@re.ru", "22222", 2, role1);
                Models.User user2 = new Models.User("email3@re.ru", "33333", 2, role1);

                dbContext.Roles.Add(role0);
                dbContext.Roles.Add(role1);
                dbContext.Users.Add(user0);
                dbContext.Users.Add(user1);
                dbContext.Users.Add(user2);
                dbContext.SaveChanges();
            }*/


        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Models.User user;
                var options = new DbContextOptionsBuilder<ApplicationContext>()
                    .UseSqlite("Data Source=data.db").Options;
                using (var dbContext = new ApplicationContext(options))
                {
                    user = dbContext.Users.Include(u => u.Role).ToList().FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                }
                
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректный логин или пароль");
            }
            return View(model);
        }

        public async Task Authenticate(Models.User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

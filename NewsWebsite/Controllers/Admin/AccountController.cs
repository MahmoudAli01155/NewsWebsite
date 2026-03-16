using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Data;
using NewsWebsite.Models;
using NewsWebsite.ViewModels;

namespace NewsWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var conn = _context.Database.GetDbConnection().ConnectionString;
            //var users = await _context.Users.ToListAsync();
            //var user1 = await _context.Users
            //.FirstOrDefaultAsync(u => u.UserName.Trim() == model.UserName.Trim());
            //var user2 = await _context.Users
            //.FirstOrDefaultAsync(u => u.UserName.Contains(model.UserName));
            //var users3 = await _context.Users.Select(u => u.UserName).ToListAsync();
            //if (!ModelState.IsValid) return View(model);

            //var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync((ApplicationUser)user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Articles");
                }
            }

            ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور غير صحيحة");
            return View(model);
        }

        // POST: Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
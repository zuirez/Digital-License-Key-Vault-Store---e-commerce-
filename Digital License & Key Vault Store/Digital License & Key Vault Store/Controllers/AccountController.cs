using Digital_License___Key_Vault_Store.Data;
using Digital_License___Key_Vault_Store.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;
using Digital_License___Key_Vault_Store.Models;
using Microsoft.EntityFrameworkCore;

namespace Digital_License___Key_Vault_Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly KeyVaultDbContext _context;
        private readonly PasswordHasher<User> _password = new();

        public AccountController(KeyVaultDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Rolename == "Customer");
            if (customerRole == null)
            {
                ModelState.AdddModelError("", "Customer role is missing in the database.");
            }

            var user = new User()
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim().ToLower(),
                RoleId = customerRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account is created successfully. Please log in.";
            return RedirectToAction(nameof(Login));
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
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var normalizeEmail=model.Email.Trim().ToLower();

            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email.ToLower()==normalizedEmail);

            if (user == null || !user.IsActive)
            {
                ModelState.AddModelError("","Invalid email or password.");
            }


        }

    }
}

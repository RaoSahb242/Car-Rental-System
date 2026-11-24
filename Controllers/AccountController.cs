using Car_Rental_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Rental_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string Email,
            string Password,
            bool RememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(
                Email,
                Password,
                RememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(Email);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }

                    if (roles.Contains("Customer"))
                    {
                        return RedirectToAction("Index", "Customer");
                    }

                    await _signInManager.SignOutAsync();

                    ViewBag.Error = "Your account does not have an assigned role.";
                    return View();
                }
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string FullName,
            string Email,
            string PhoneNumber,
            string Password,
            string ConfirmPassword)
        {
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = FullName,
                UserName = Email,
                Email = Email,
                PhoneNumber = PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Customer"))
                {
                    await _roleManager.CreateAsync(
                        new IdentityRole("Customer")
                    );
                }

                var roleResult = await _userManager.AddToRoleAsync(
                    user,
                    "Customer"
                );

                if (roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(
                        user,
                        isPersistent: false
                    );

                    return RedirectToAction("Index", "Customer");
                }

                await _userManager.DeleteAsync(user);

                ViewBag.Error = "Unable to assign Customer role.";
                return View();
            }

            ViewBag.Error = string.Join(
                " ",
                result.Errors.Select(e => e.Description)
            );

            return View();
        }

        [HttpGet]
        public IActionResult AdminRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegister(
            string FullName,
            string Email,
            string PhoneNumber,
            string Password,
            string ConfirmPassword,
            string SetupCode)
        {
            string correctSetupCode =
                _configuration["AdminRegistration:SetupCode"];

            if (string.IsNullOrEmpty(correctSetupCode) ||
                SetupCode != correctSetupCode)
            {
                ViewBag.Error = "Invalid Admin Setup Code.";
                return View();
            }

            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = FullName,
                UserName = Email,
                Email = Email,
                PhoneNumber = PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    var createRole = await _roleManager.CreateAsync(
                        new IdentityRole("Admin")
                    );

                    if (!createRole.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);

                        ViewBag.Error = "Unable to create Admin role.";
                        return View();
                    }
                }

                var roleResult = await _userManager.AddToRoleAsync(
                    user,
                    "Admin"
                );

                if (roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(
                        user,
                        isPersistent: false
                    );

                    return RedirectToAction("Index", "Admin");
                }

                await _userManager.DeleteAsync(user);

                ViewBag.Error = "Unable to assign Admin role.";
                return View();
            }

            ViewBag.Error = string.Join(
                " ",
                result.Errors.Select(e => e.Description)
            );

            return View();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.Data;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            var loginVM = new LoginViewModel();
            return View(loginVM);
        }


        /// <summary>
        /// Logs a user in and redirects to Dashboard
        /// </summary>
        /// <param name="loginViewModel"></param>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                // user found
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    // check password
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                // password incorrect
                TempData["Error"] = "Password doesn't match the user.";
                return View(loginViewModel);
            }
            // User not found
            TempData["Error"] = "User not found.";
            return View(loginViewModel);
        }

        /// <summary>
        /// Generates the form for registering a user
        /// </summary>
        public IActionResult Register()
        {
            var registerVM = new RegisterViewModel();
            return View(registerVM);
        }


        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="registerViewModel"></param>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            // Check for existing user
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new User()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress,
                Name = registerViewModel.Name,
                DateRegistered = DateTime.Now,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            // assign role to new user
            if (newUserResponse.Succeeded)
            {
                if (registerViewModel.RoleSelected != null && registerViewModel.RoleSelected.Length > 0 && registerViewModel.RoleSelected == "teacher")
                {
                    await _userManager.AddToRoleAsync(newUser, "teacher");
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, "student");
                }
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);

                return RedirectToAction("Index", "Home");

            }
            else if (!newUserResponse.Succeeded)
                TempData["Error"] = "Enter a more secure password";

            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

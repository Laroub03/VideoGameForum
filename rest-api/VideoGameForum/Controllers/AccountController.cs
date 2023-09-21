using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VideoGameForum.Models;
using VideoGameForum.ViewModels;

namespace VideoGameForum.Controllers
{
    // This attribute specifies that the controller requires authentication
    [Authorize]
    public class AccountController : Controller
    {
        // User manager and sign-in manager provide user management and sign-in functionality
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        // Constructor for injecting the user manager and sign-in manager
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Endpoint for the registration view
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register() => View();

        // Endpoint for handling user registration
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                // If user is successfully created, sign them in
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Forum");
                }

                // Add any errors that occurred during registration to the model state
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // Endpoint for the login view
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        // Endpoint for handling user login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                // If login is successful, redirect to forum index
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Forum");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        // Endpoint for handling user logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
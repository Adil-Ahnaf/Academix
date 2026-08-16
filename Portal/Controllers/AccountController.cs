using AspNetCore.Identity.Dapper.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portal.ViewModels;
using Newtonsoft.Json;

namespace Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(LoginViewModel model, string recaptchaResponse)
        {
            if (!ModelState.IsValid)
                return View("Login", model);

            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && user.IsActive == true)
            {
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("Login", "Email not verified! Please check your inbox/spam folder to verify email.");
                    return View("Login", model);
                }

                var signInresult = await _signInManager.PasswordSignInAsync(user, model.Password, model.IsPersistentCookie, true);
                
                if (signInresult.Succeeded)
                {
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.IsPersistentCookie,
                        ExpiresUtc = DateTimeOffset.Now.AddMinutes(30)
                    };

                    await _signInManager.SignInAsync(user, authProperties);

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && model.ReturnUrl != "Home")
                        return Redirect(model.ReturnUrl);

                    return RedirectToAction("Index", "Home");
                }
                else if (signInresult.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "Account is locked out.");
                    return View("Login", model);
                }
                else
                {
                    ModelState.AddModelError("Login", "Login Failed. Password incorrect.");
                    return View("Login", model);
                }
            }
            else
            {
                ModelState.AddModelError("Login", "Login Failed. Please try again.");
                return View("Login", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.Web.Models;

namespace CurriculumSharingPortal.Web.Controllers
{
	public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(vm.UserName);
                if(user == null)
                {
                    ModelState.AddModelError("", "Unsuccessfull login!");
                    return View(vm);
                }

                var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Unsuccessfull login!");
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { 
                    DisplayName = vm.DisplayName,
                    UserName = vm.UserName,
                    Email = vm.Email,
                };
                var result = await _userManager.CreateAsync(user, vm.Password);

                await _userManager.AddToRoleAsync(user, "User");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("", "Unsuccessful registration!");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                if (User.Identity?.Name == null && returnUrl.Contains(nameof(CurriculumController.DownloadFile)))
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    return Redirect(returnUrl);

                }
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}

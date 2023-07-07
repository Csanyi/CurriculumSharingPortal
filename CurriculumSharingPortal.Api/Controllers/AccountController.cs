using CurriculumSharingPortal.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CurriculumSharingPortal.Api.Controllers
{
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null) { return Unauthorized("Login failed!"); }

            if(!(await _userManager.IsInRoleAsync(user, "Admin"))) { return Unauthorized("You have to be an admin to login!"); }

            var result = await _signInManager.PasswordSignInAsync(login.UserName ,login.Password, false, false);

            if(result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized("Login failed!");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}

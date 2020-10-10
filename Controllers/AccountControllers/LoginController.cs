using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pd_api.Models;
using pd_api.Models.Account;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    public class LoginController : Controller
    {
        private SignInManager<AppUser> signInManager;
        //private PasswordHasher<AppUser> passwordHasher;
        private UserManager<AppUser> userManager;

        public LoginController(SignInManager<AppUser> signInMgr,
            //PasswordHasher<AppUser> passwordHash,
            UserManager<AppUser> userMgr)
        {
            signInManager = signInMgr;
            //passwordHasher = passwordHash;
            userManager = userMgr;
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginModel loginData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(loginData.UserName);
                if (user != null)
                {
                    if (true)//await userManager.IsEmailConfirmedAsync(user))
                    {
                        Microsoft.AspNetCore.Identity.SignInResult signInResult =
                            await signInManager.PasswordSignInAsync(user, loginData.Password, true, false);
                        if (signInResult.Succeeded)
                        {
                            return Json(signInResult);
                        }
                        else
                        {
                            return Json(signInResult);
                        }
                    }
                    else
                    {
                        return Json(new { succeeded = false, messageInfo = "Email address is not confirmed." });
                    }
                }
                else
                {
                    return Json(new { succeeded = false, messageInfo = "Could not find user." });
                }
            }
            else
            {
                return Json(ModelState);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Json(new { succeeded = true });
        }
    }
}

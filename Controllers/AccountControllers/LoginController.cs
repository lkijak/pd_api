﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pd_api.Models.DbModel;
using pd_api.Models.ViewModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    [Route("Login")]
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
        [AllowAnonymous]
        public async Task<JsonResult> Login([FromBody] LoginViewModel loginData)
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
                        return Json(new { succeeded = false, messageInfo = MessageInfo.Login_EmailAddress_NotConfirmed });
                    }
                }
                else
                {
                    return Json(new { succeeded = false, messageInfo = MessageInfo.Login_CouldNotFindUser });
                }
            }
            else
            {
                return Json(ModelState);
            }
        }

        [HttpGet("GoogleLogin")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = Url.Action("GoogleResponse", "Login");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet("GoogleResponse")]
        [AllowAnonymous]
        public async Task<JsonResult> GoogleResponse()
        {
            ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return Json(new { succeeded = false, messageInfo = MessageInfo.Login_GoogleAccountDontExist });
            }
            
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
            {
                return Json(userInfo);
            }
            else
            {
                AppUser user = new AppUser
                {
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    Name = info.Principal.FindFirst(ClaimTypes.Name).Value
                };

                IdentityResult identResult = await userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return Json(userInfo);
                    }
                }
                return Json(new { succeeded = false, messageInfo = MessageInfo.Login_AccessDenied });
            }
        }
    }
}

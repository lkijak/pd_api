using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using pd_api.Models;
using pd_api.Models.Account;
using pd_api.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{

    public class RegistrationController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private IConfiguration configuration;

        public RegistrationController(UserManager<AppUser> userMgr,
            SignInManager<AppUser> signMgr,
            IConfiguration config,
            IUserValidator<AppUser> userValid,
            IPasswordHasher<AppUser> passwordHash)
        {
            userManager = userMgr;
            signInManager = signMgr;
            configuration = config;
            userValidator = userValid;
            passwordHasher = passwordHash;
        }

        [HttpPost]
        public async Task<JsonResult> CreateAccount([FromBody] Registration registrationData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = registrationData.UserName,
                    Email = registrationData.Email
                };

                IdentityResult createResult = await userManager.CreateAsync(user, registrationData.Password);
                if (createResult.Succeeded)
                {
                    //tutaj dodać metodę do wysyłania email potwierdzającego adres

                    return Json(createResult);
                }
                else
                {
                    return Json(createResult);
                }
            }
            return Json(ModelState);
        }

        [HttpPatch]
        public async Task<JsonResult> EditAccount([FromBody] Registration registrationData)
        {
            return Json("OK");
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteAccount([FromBody] Login deleteData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(deleteData.UserName);
                if (user != null)
                {
                    if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, deleteData.Password) ==
                        PasswordVerificationResult.Failed)
                    {
                        return Json(new RequestError(false, "Wrong password."));
                    }
                    else
                    {
                        IdentityResult deleteResult = await userManager.DeleteAsync(user);
                        if (deleteResult.Succeeded)
                        {
                            return Json(deleteResult);
                        }
                        else
                        {
                            return Json(deleteResult);
                        }
                    }
                }
                return Json(new RequestError(false, "Could not find user."));
            }
            else
            {
                return Json(ModelState);
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using pd_api.Models;
using pd_api.Models.Account;
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

        [HttpGet]
        public async Task<JsonResult> GetUserAccount([FromBody] string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                AppUser user = await userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var config = new MapperConfiguration(config => config.CreateMap<AppUser, ShowUserAccountModel>());
                    var mapper = new Mapper(config);
                    ShowUserAccountModel showAccount = mapper.Map<ShowUserAccountModel>(user);
                    return Json(showAccount);
                }
                else
                {
                    return Json(new { succeeded = false, messageInfo = "Could not find user." });
                }
            }
            else
            {
                return Json(new { succeeded = false, messageInfo = "Didn't pass the username" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateAccount([FromBody] RegistrationModel registrationData)
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
                    //i  ustawić potwierdzenie adresu w setup

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
        public async Task<JsonResult> EditAccount([FromBody] EditAccountModel editAccountData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(editAccountData.UserName);
                if (user != null)
                {
                    if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, editAccountData.Password) ==
                        PasswordVerificationResult.Failed)
                    {
                        return Json(new { succeeded = false, messageInfo = "Wrong password." });
                    }
                    else
                    {
                        user.Name = editAccountData.Name;
                        user.Lastname = editAccountData.Lastname;
                        user.PostCode = editAccountData.PostCode;
                        user.City = editAccountData.City;
                        user.Address = editAccountData.Address;

                        IdentityResult editResult = await userManager.UpdateAsync(user);
                        if (editResult.Succeeded)
                        {
                            return Json(editResult);
                        }
                        else
                        {
                            return Json(editResult);
                        }
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

        [HttpDelete]
        public async Task<JsonResult> DeleteAccount([FromBody] LoginModel deleteData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(deleteData.UserName);
                if (user != null)
                {
                    if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, deleteData.Password) ==
                        PasswordVerificationResult.Failed)
                    {
                        return Json(new { succeeded = false, messageInfo = "Wrong password." });
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
                return Json(new { succeeded = false, messageInfo = "Could not find user." });
            }
            else
            {
                return Json(ModelState);
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using pd_api.Models;
using pd_api.Models.Account;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private IConfiguration configuration;

        public UserController(UserManager<AppUser> userMgr,
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

        [HttpGet("Users")]
        public JsonResult GetAllUsers()
        {
            var users = from u in userManager.Users
                        select new
                        {
                            UserName = u.UserName,
                            Email = u.Email,
                            Name = u.Name,
                            Lastname = u.Lastname,
                            PostCode = u.PostCode,
                            City = u.City,
                            Address = u.Address
                        };

            if (users != null)
            {
                return Json(users);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<JsonResult> GetUser(string userName)
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
                    return Json(new { succeeded = false, messageInfo = MessageInfo.User_CouldNotFindUser });
                }
            }
            else
            {
                return Json(new { succeeded = false, messageInfo = MessageInfo.User_DidntPassUserName });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUser([FromBody] RegistrationModel registrationData)
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
        public async Task<JsonResult> EditUser([FromBody] EditUserModel editAccountData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(editAccountData.UserName);
                if (user != null)
                {
                    if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, editAccountData.Password) ==
                        PasswordVerificationResult.Failed)
                    {
                        return Json(new { succeeded = false, messageInfo = MessageInfo.User_WrongPassword });
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
                    return Json(new { succeeded = false, messageInfo = MessageInfo.User_CouldNotFindUser });
                }
            }
            else
            {
                return Json(ModelState);
            }
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteUser([FromBody] LoginModel deleteData)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(deleteData.UserName);
                if (user != null)
                {
                    if (passwordHasher.VerifyHashedPassword(user, user.PasswordHash, deleteData.Password) ==
                        PasswordVerificationResult.Failed)
                    {
                        return Json(new { succeeded = false, messageInfo = MessageInfo.User_WrongPassword });
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
                return Json(new { succeeded = false, messageInfo = MessageInfo.User_CouldNotFindUser });
            }
            else
            {
                return Json(ModelState);
            }
        }
    }
}

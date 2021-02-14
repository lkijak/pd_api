using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using pd_api.Models;
using pd_api.Models.DbModel;
using pd_api.Models.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private UserManager<AppUser> userManager;
        private AppDbContext context; 

        public UserController(UserManager<AppUser> userMgr,
            SignInManager<AppUser> signMgr,
            IConfiguration config,
            IUserValidator<AppUser> userValid,
            IPasswordHasher<AppUser> passwordHash,
            AppDbContext ctx)
        {
            userManager = userMgr;
            context = ctx;
        }

        [HttpGet("Users")]
        public IActionResult GetAllUsers()
        {
            IQueryable<UserAccountViewModel> users = null;
            if (userManager.Users.Any())
            {
                users = from u in userManager.Users
                        select new UserAccountViewModel
                        {
                            UserName = u.UserName,
                            Email = u.Email,
                            Name = u.Name,
                            Lastname = u.Lastname,
                            PostCode = u.PostCode,
                            City = u.City,
                            Address = u.Address
                        };
            }
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return ValidationProblem(MessageInfo.User_DidntPassUserName);
            }

            AppUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }

            var config = new MapperConfiguration(config => config.CreateMap<AppUser, UserAccountViewModel>());
            var mapper = new Mapper(config);
            UserAccountViewModel showAccount = mapper.Map<UserAccountViewModel>(user);
            var survey = context.UserResponses.FirstOrDefault(r => r.SurveyName == MessageInfo.Survey_LifeCircle && r.UserId == user.Id);
            if (survey != null)
            {
                showAccount.IsLifeCircleFilled = true;
            }
            else
            {
                showAccount.IsLifeCircleFilled = false;
            }
            var roles = await userManager.GetRolesAsync(user);
            showAccount.Role = roles;
            return Ok(showAccount);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegistrationViewModel registrationData)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            AppUser user = new AppUser
            {
                UserName = registrationData.UserName,
                Email = registrationData.Email,
                EmailConfirmed = true
            };

            IdentityResult createResult = await userManager.CreateAsync(user);
            IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, MessageInfo.Role_User);
            if (createResult.Succeeded && addToRoleResult.Succeeded)
            {
                return Ok(createResult);
            }

            return Conflict(createResult);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return ValidationProblem(MessageInfo.User_DidntPassUserName);
            }

            AppUser user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }
                
            IdentityResult deleteResult = await userManager.DeleteAsync(user);
            return Ok(deleteResult);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pd_api.Models;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    [Route("Logout")]
    public class LogoutController : Controller
    {
        private SignInManager<AppUser> signInManager;

        public LogoutController(SignInManager<AppUser> signInMgr)
        {
            signInManager = signInMgr;
        }

        [HttpPost]
        public async Task<JsonResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Json(new { succeeded = true });
        }
    }
}

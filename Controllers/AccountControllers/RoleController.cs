using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pd_api.Models.DbModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private RoleManager<AppRole> roleManager;
        private ControllerErrorHandler handler = new ControllerErrorHandler();

        public RoleController(RoleManager<AppRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        [HttpGet("Roles")]
        public IActionResult GetRoles()
        {
            IQueryable<AppRole> roles = roleManager.Roles;
            if (roles.Any())
            {
                return Ok(roles);
            }
            return Ok(new string[0]);
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(string name)
        {
            try
            {
                AppRole role = await roleManager.FindByNameAsync(name);
                if (role != null)
                {
                    return Ok(role);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return NotFound(String.Format(MessageInfo.Error_NotFound, name));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            AppRole role = new AppRole
            {
                Name = name
            };

            IdentityResult result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return Created("", role);
            }
            else
            {
                return handler.IdentityResultError(result);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string name)
        {
            IdentityResult result;
            AppRole role = await roleManager.FindByNameAsync(name);
            try
            {
                result =  await roleManager.DeleteAsync(role);
            }
            catch (Exception)
            {
                throw;
            }

            if (result.Succeeded)
            {
                return Ok(null);
            }
            else
            {
                return handler.IdentityResultError(result);
            }
        }
    }
}

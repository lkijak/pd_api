using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pd_api.Models.DbModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.AccountControllers
{
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private RoleManager<AppRole> roleManager;

        public RoleController(RoleManager<AppRole> roleMgr)
        {
            roleManager = roleMgr;
        }

        [HttpGet("Roles")]
        public IActionResult GetRoles()
        {
            IList<string> roles = null;
            if (roleManager.Roles.Any())
            {
                roles = new List<string>();
                foreach (var role in roleManager.Roles)
                {
                    roles.Add(role.Name);
                }
            }
            return Ok(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(string name)
        {
            AppRole role = null;
            if (roleManager.Roles.Any())
            {
                role = await roleManager.FindByNameAsync(name);
            }
            return Ok(role.Name);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                AppRole role = new AppRole
                {
                    Name = name
                };
                await roleManager.CreateAsync(role);
                return Created("/Role", role);
            }
            return ValidationProblem("Name value is empty");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = null;
                AppRole role = await roleManager.FindByNameAsync(name);
                if (role != null)
                {
                    result = await roleManager.DeleteAsync(role);
                    return Ok();
                }
                return Ok(role);
            }
            return ValidationProblem("Name value is empty");
        }
    }
}

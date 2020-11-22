using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pd_api.Models.User;
using System;
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
            IQueryable<AppRole> roles = roleManager.Roles;
            if (roles.Any())
            {
                return Ok(roles);
            }
            return NotFound(MessageInfo.Error_NotFoundAny);
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
            //var result = await roleManager.CreateAsync(role);
            //if (result.Succeeded)
            //{
            //    return Created("", role);
            //}
            //return NotFound(result);

            try
            {
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Created("", role);
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok();
        }









    }
}

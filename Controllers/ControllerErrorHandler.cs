using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers
{
    public class ControllerErrorHandler : Controller
    {
        private Dictionary<string, string> errorList;

        public IActionResult IdentityResultError(IdentityResult result)
        {
            errorList = new Dictionary<string, string>();

            foreach (var item in result.Errors)
            {
                errorList.Add(item.Code, item.Description);
            }

            foreach (var item in errorList)
            {
                if (item.Key == "")
                {

                }
            }

            return Ok(errorList);

        }



    }
}

using Microsoft.AspNetCore.Mvc;
using pd_api.Models;
using pd_api.Models.DbModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace pd_api.Controllers.EmailControllers
{
    [Route("ConfigurationEmail")]
    public class ConfigurationEmailController : Controller
    {
        private AppDbContext context;

        public ConfigurationEmailController(AppDbContext ctx)
        {
            context = ctx;
        }

        [HttpGet]
        public JsonResult GetConfiguration()
        {
            try
            {
                EmailConfiguration model = context.EmailConfigurations.First();
                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(new { succeeded = false, messageInfo = ex.ToString() });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateConfiguration([FromBody] EmailConfiguration configurationData)
        {
            if (context.EmailConfigurations == null)
            {
                return Json(new { succeeded = false, messageInfo = MessageInfo.Email_ConfigurationRecordAlreadyExist });
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await context.EmailConfigurations.AddAsync(configurationData);
                    await context.SaveChangesAsync();
                    return Json(new { succeeded = true });
                }
                catch (Exception ex)
                {
                    return Json(new { exception = ex.ToString() });
                }
            }
            else
            {
                return Json(ModelState);
            }
        }

        [HttpPatch]
        public JsonResult EditConfiguration([FromBody] EmailConfiguration configurationData)
        {
            EmailConfiguration emeilConfig = context.EmailConfigurations.FirstOrDefault();
            if (emeilConfig != null)
            {
                emeilConfig.FriendlyName = configurationData.FriendlyName;
                emeilConfig.Login = configurationData.Login;
                emeilConfig.Password = configurationData.Password;
                emeilConfig.UseDefaultCredential = configurationData.UseDefaultCredential;
                emeilConfig.Host = configurationData.Host;
                emeilConfig.Port = configurationData.Port;
                emeilConfig.EnableSSL = configurationData.EnableSSL;
                emeilConfig.DefaultMessageBody = configurationData.DefaultMessageBody;
                try
                {
                    context.EmailConfigurations.Update(emeilConfig);
                    return Json(new { succeeded = true });
                }
                catch (Exception ex)
                {
                    return Json(new { exception = ex.ToString() });
                }
            }
            return Json(new { succeeded = false, messageInfo = MessageInfo.Email_CouldNotFindConfiguration });
        }
    }
}

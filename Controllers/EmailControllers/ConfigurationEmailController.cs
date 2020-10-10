using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pd_api.Models.Email;
using pd_api.Service;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace pd_api.Controllers.EmailControllers
{
    public class ConfigurationEmailController : Controller
    {
        private AppDbContext context;
        private readonly ILogger logger;

        public ConfigurationEmailController(AppDbContext ctx,
            ILogger<ConfigurationEmailController> log)
        {
            context = ctx;
            logger = log;
        }

        [HttpGet("GetConfiguration")]
        public JsonResult GetConfiguration()
        {
            logger.LogInformation("GET Controller called");
            try
            {
                EmailConfigurationModel model = context.EmailConfigurations.First();
                return Json(model);
            }
            catch (Exception ex)
            {
                logger.LogError("Log from GetConfiguration().", ex);
                return Json(new { succeeded = false, messageInfo = ex.ToString() });
            }
        }

        [HttpPost("SetConfiguration")]
        public async Task<JsonResult> SetConfiguration([FromBody] EmailConfigurationModel configurationData)
        {
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
    }
}

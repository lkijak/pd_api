using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pd_api.Models;
using pd_api.Models.DbModel;
using pd_api.Models.ViewModel;
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
        public IActionResult GetConfiguration()
        {
            EmailConfigurationViewModel model = null;
            if (context.EmailConfigurations.Any())
            {
                EmailConfiguration getModel = context.EmailConfigurations.FirstOrDefault();

                var config = new MapperConfiguration(config => config.CreateMap<EmailConfiguration, EmailConfigurationViewModel>());
                var mapper = new Mapper(config);
                model = mapper.Map<EmailConfigurationViewModel>(getModel);
            }
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateConfiguration([FromBody] EmailConfigurationViewModel configurationData)
        {
            if (ModelState.IsValid)
            {
                if (context.EmailConfigurations.Any())
                {
                    return Conflict("Configuration data already exists");
                }

                var config = new MapperConfiguration(config => config.CreateMap<EmailConfigurationViewModel, EmailConfiguration>());
                var mapper = new Mapper(config);
                EmailConfiguration emailConfiguration = mapper.Map<EmailConfiguration>(configurationData);

                await context.EmailConfigurations.AddAsync(emailConfiguration);
                await context.SaveChangesAsync();
                return Created("/EmailConfiguration", configurationData);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPatch]
        public async Task<IActionResult> EditConfiguration([FromBody] EmailConfigurationViewModel configurationData)
        {
            if (ModelState.IsValid)
            {
                EmailConfiguration emailConfig = null;
                if (context.EmailConfigurations.Any())
                {
                    emailConfig = context.EmailConfigurations.FirstOrDefault();

                    var config = new MapperConfiguration(config => config.CreateMap<EmailConfigurationViewModel, EmailConfiguration>());
                    var mapper = new Mapper(config);
                    EmailConfiguration newEmailConfiguration = mapper.Map<EmailConfiguration>(configurationData);

                    emailConfig = newEmailConfiguration;
                    context.EmailConfigurations.Update(emailConfig);
                    await context.SaveChangesAsync();
                }
                return Ok(configurationData);
            }
            return ValidationProblem(ModelState);
        }
    }
}

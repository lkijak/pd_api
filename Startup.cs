using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pd_api.Models;
using pd_api.Models.User;
using pd_api.Service;

namespace pd_api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Database:pd_api_DBLocal:ConnectionString"]));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //services.AddAuthentication()
            //    //.AddGoogle(options =>
            //    //{
            //    //    IConfigurationSection googleAuthNSection =
            //    //        Configuration.GetSection("Authentication:Google");

            //    //    options.ClientId = googleAuthNSection["ClientId"];
            //    //    options.ClientSecret = googleAuthNSection["ClientSecret"];
            //    //});
            //    .AddMicrosoftAccount(microsoftOptions =>
            //    {
            //        microsoftOptions.ClientId = "d6af278e-39b2-449f-8962-27f37b0b2576"; //Configuration["Authentication:Microsoft:ClientId"];
            //        microsoftOptions.ClientSecret = "4~xV9Wk.Uu.4Z_r8jXwjMcrdut6iLY5Im."; //Configuration["Authentication:Microsoft:ClientSecret"];
            //    });

            services.AddControllers();

            services.AddSwaggerGen(info =>
            {
                info.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "SMART Evolution API",
                    Version = "v1.0",
                    Description = "Aplikacja stworzona w ramach projektu dyplomowego"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "SMART Evolution");

                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseAuthentication();
        }
    }
}

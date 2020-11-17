using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using pd_api.Models;
using pd_api.Models.User;
using pd_api.Models.Email;

namespace pd_api.Service
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EmailConfigurationModel> EmailConfigurations { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using pd_api.Models.DbModel;

namespace pd_api.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserResponse> UserResponses { get; set; }
        public virtual DbSet<UserResponseQuestionAndAnswer> UserResponseQuestionsAndAnswers { get; set; }
        public virtual DbSet<UserTask> UserTasks { get; set; }
        public virtual DbSet<UserSubtask> UserSubtasks { get; set; }
    }
}

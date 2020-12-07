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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Dictionary>()
                .HasIndex(i => i.Name)
                .HasDatabaseName("Dictionaries_Index_Name");

            builder.Entity<Dictionary>()
                .Property(b => b.DictionaryData)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));

            builder.Entity<Survey>()
                .HasIndex(i => i.Name)
                .HasDatabaseName("Survey_Index_Name")
                .IsUnique();
        }

        public virtual DbSet<EmailConfiguration> EmailConfigurations { get; set; }
        public virtual DbSet<Dictionary> Dictionaries { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<SurveyOferedAnswer> SurveyOferedAnswers { get; set; }
        public virtual DbSet<UserResponse> UserResponses { get; set; }
        public virtual DbSet<UserResponseQuestionAndAnswer> UserResponseQuestionsAndAnswers { get; set; }
    }
}

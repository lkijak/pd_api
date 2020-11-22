using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using pd_api.Models;
using pd_api.Models.User;
using pd_api.Models.Email;
using pd_api.Models.Dictionary;
using Newtonsoft.Json;
using System.Collections.Generic;
using pd_api.Models.Survey;

namespace pd_api.Service
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

            builder.Entity<DictionaryModel>()
                .HasIndex(i => i.Name)
                .HasDatabaseName("Dictionaries_Index_Name");

            builder.Entity<DictionaryModel>()
                .Property(b => b.Dictionary)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));

            builder.Entity<SurveyModel>()
                .HasIndex(i => i.Name)
                .HasDatabaseName("Survey_Index_Name")
                .IsUnique();
        }

        public virtual DbSet<EmailConfigurationModel> EmailConfigurations { get; set; }
        public virtual DbSet<DictionaryModel> Dictionaries { get; set; }
        public virtual DbSet<SurveyModel> Surveys { get; set; }
        public virtual DbSet<QuestionModel> Questions { get; set; }
        public virtual DbSet<AnswerModel> Answers { get; set; }
        public virtual DbSet<SurveyResponseModel> SurveyResponses { get; set; }
        public virtual DbSet<ResponseModel> Responses { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace RaceAnalysis.Models
{
    public class RaceAnalysisDbContext : IdentityDbContext<ApplicationUser>
    {

        public RaceAnalysisDbContext() : base("name=RaceAnalysis",throwIfV1Schema: false )//connection string      
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RaceAnalysisDbContext, 
                                          RaceAnalysis.Migrations.Configuration>("RaceAnalysis"));

            //Database.SetInitializer(new Data.RaceAnalysisDBInitializer());

            this.Configuration.LazyLoadingEnabled = true;//just want to explicitly control this
        }

        public static RaceAnalysisDbContext Create()
        {
            return new RaceAnalysisDbContext();
        }

        public DbSet<Triathlete> Triathletes { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<RequestContext> RequestContext { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<RaceAnalysis.Models.RaceConditions> RaceConditions { get; set; }
    }
}
namespace RaceAnalysis.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using RaceAnalysis.Models;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<RaceAnalysis.Models.RaceAnalysisDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            // Set the |DataDirectory| path used in connection strings to point to the correct directory 
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relative = @"..\SharedRaceAnalysis\App_Data\";
            string absolute = Path.GetFullPath(Path.Combine(baseDirectory, relative));
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }

        protected override void Seed(RaceAnalysisDbContext context)
        {
            RaceAnalysis.Data.RaceAnalysisDbInitializer.Seed(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}

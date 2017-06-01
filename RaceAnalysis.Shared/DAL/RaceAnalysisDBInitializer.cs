using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RaceAnalysis.Models;
using System.Data.Entity.Migrations;

namespace RaceAnalysis.Data
{
    public class RaceAnalysisDbInitializer
    {
        public static void Seed(RaceAnalysisDbContext context)
        {
           
            return;

            //if (context.Triathletes.Count(i => i.TriathleteId > 1) == 0)
            {

                SeedRaces(context);

                SeedGenders(context);

                SeedAgeGroups(context);

                /*** only do for testing if absolutely necessary**********
                //SeedRequestContexts(context);
                //SeedTriathletes(context);
                ************************************/
                SeedAppFeatures(context);

                SeedRaceConditionTag(context);

                SeedTags(context);
             }

        }//seed

        private static void SeedTriathletes(RaceAnalysisDbContext context)
        {
            if (context.Triathletes.Count(i => i.TriathleteId > 1) == 0)
            {
                var athletes = new List<Triathlete>
                    {
                        new Triathlete
                        {
                            RequestContextId= context.RequestContext.First().RequestContextId,
                            Link="",
                            Name="Scott",
                            Country="USA",
                            DivRank=1,
                            GenderRank=1,
                            OverallRank=1,
                            Swim= new TimeSpan(1,30,0),
                            Bike= new TimeSpan(4,30,0),
                            Run= new TimeSpan(3,30,0),
                            Finish= new TimeSpan(1,30,0),
                            Points = 100


                        }
                    };


                athletes.ForEach(t => context.Triathletes.AddOrUpdate(t));
                context.SaveChanges();
            }
        }

        private static void SeedRequestContexts(RaceAnalysisDbContext context)
        {
            context.RequestContext.RemoveRange(context.RequestContext);
            context.SaveChanges();

            var contextkeys = new List<RequestContext>
                {
                    new RequestContext
                    {

                        RaceId= context.Races.Single(i => i.DisplayName == "IM Louisville 10-11-2015").RaceId,
                        GenderId=context.Genders.Single(i => i.DisplayName == "Male").GenderId,
                        AgeGroupId= context.AgeGroups.Single(i => i.DisplayName == "50-54").AgeGroupId
                    }

                };
            contextkeys
                .ForEach(t => context.RequestContext.AddOrUpdate(t));
            context.SaveChanges();
        }

        private static void SeedRaces(RaceAnalysisDbContext context)
        {
            context.Races.RemoveRange(context.Races); //clear out existing rows
            context.SaveChanges();
            var races = new List<Race>
                            {

                                new Race
                                {
                                    RaceId="IMLOU2015",
                                    BaseURL="http://www.ironman.com/triathlon/events/americas/ironman/louisville/results.aspx",
                                    DisplayName="IMLOU 2015",
                                    RaceDate = new DateTime(2015,10,11),
                                    ShortName="louisville",
                                    Distance = "140.6",
                                    Conditions = new RaceConditions
                                    {

                                    }
                                    //Conditions = new RaceConditions {SwimLayout="Wetsuit Legal",BikeLayout="Rolling Hills",RunLayout="Flat" }

                                },
                                 new Race
                                {
                                    RaceId="IMLOU2016",
                                    BaseURL= "http://www.ironman.com/triathlon/events/americas/ironman/louisville/results.aspx",
                                    DisplayName ="IMLOU 2016",
                                    RaceDate = new DateTime(2016,10,9),
                                    ShortName="louisville",
                                    Distance = "140.6",
                                    Conditions = new RaceConditions
                                    {

                                    }
                                },
                                  new Race
                                {
                                    RaceId="IMFL2015",
                                    BaseURL="http://www.ironman.com/triathlon/events/americas/ironman/florida/results.aspx",
                                    DisplayName="IMFL 2015",
                                    RaceDate = new DateTime(2015,11,7),
                                    ShortName="florida",
                                    Distance = "140.6",
                                    Conditions = new RaceConditions
                                    {

                                    }
                                }


                            };

            races.ForEach(t => context.Races.AddOrUpdate(t));
            context.SaveChanges();
        }

        private static void SeedGenders(RaceAnalysisDbContext context)
        {
            context.Genders.RemoveRange(context.Genders);
            context.SaveChanges();
            var genders = new List<Gender>
                {
                    new Gender
                    {
                        GenderId=1,
                        DisplayName="Male",
                        Value="M"
                    },
                    new Gender
                    {
                        GenderId=2,
                        DisplayName="Female",
                        Value = "F"
                    }
                };
            genders.ForEach(t => context.Genders.AddOrUpdate(t));
            context.SaveChanges();
        }

        private static void SeedAgeGroups(RaceAnalysisDbContext context)
        {
            context.AgeGroups.RemoveRange(context.AgeGroups);
            context.SaveChanges();

            var agegroups = new List<AgeGroup>
            {
                new AgeGroup
                {

                    DisplayName= "Pro",
                    Value="Pro"
                },

                new AgeGroup
                {
                    DisplayName= "18-24",
                    Value="18-24"
                },
                new AgeGroup
                {
                    DisplayName= "25-29",
                    Value="25-29"
                },
                new AgeGroup
                {
                    DisplayName= "30-34",
                    Value="30-34"
                },
                new AgeGroup
                {
                    DisplayName= "40-44",
                    Value="40-44"
                },
                new AgeGroup
                {
                    DisplayName= "45-49",
                    Value="45-49"
                },
                new AgeGroup
                {
                    DisplayName= "50-54",
                    Value="50-54"
                },
                new AgeGroup
                {
                    DisplayName= "55-59",
                    Value="55-59"
                },
                new AgeGroup
                {
                    DisplayName= "60-64",
                    Value="60-64"
                },

                 new AgeGroup
                {
                    DisplayName= "65-69",
                    Value="65-69"
                },
                  new AgeGroup
                {
                    DisplayName= "70-74",
                    Value="70-74"
                },
                   new AgeGroup
                {
                    DisplayName= "75-79",
                    Value="75-79"
                },
                    new AgeGroup
                {
                    DisplayName= "80-84",
                    Value="80-84"
                },
                     new AgeGroup
                {
                    DisplayName= "85-89",
                    Value="85-89"
                },
                      new AgeGroup
                {
                    DisplayName= "90 Plus",
                    Value="90+Plus"
                },
                 new AgeGroup
                {
                    DisplayName= "PC",
                    Value="PC"
                },

             };
            agegroups.ForEach(t => context.AgeGroups.AddOrUpdate(t));
            context.SaveChanges();
        }


        private static void SeedAppFeatures(RaceAnalysisDbContext context)
        {
            context.AppFeatures.RemoveRange(context.AppFeatures);
            context.SaveChanges();
            var features = new List<AppFeature>
                {
                //////////////////////////////////////////////////////////
                //Admin 
                //////////////////////////////////////////////////////////
                    new AppFeature
                    {
                        Category = FeatureCategories.Admin,
                        Name = "Add Race Data",
                        Description = "Allow admin to add race and populate cache",
                        Status = FeatureStatus.Done
                    },
                    new AppFeature
                    {
                        Category = FeatureCategories.Admin,
                        Name = "Registration",
                        Description = "Allow users to register",
                        Status = FeatureStatus.Done
                    },
                    new AppFeature
                    {
                        Category = FeatureCategories.Admin,
                        Name = "Login",
                        Description = "Allow users to login",
                        Status = FeatureStatus.Done
                    },
                   
                    ////////////////////////////////////////////////////////////
                    // Content Contribution
                    ////////////////////////////////////////////////////////////
                    new AppFeature
                    {
                        Category = FeatureCategories.ContentContrib,
                        Name = "Content Contribution",
                        Description = "Allow users to signup for content contribution",
                        Status = FeatureStatus.NotStarted
                    },
                    new AppFeature
                    {
                        Category = FeatureCategories.ContentContrib,
                        Name = "Add Race Conditions",
                        Description = "Allow users to add race conditions ",
                        Status = FeatureStatus.NotStarted
                    },
                     new AppFeature
                    {
                        Category = FeatureCategories.ContentContrib,
                        Name = "Add Race Analysis",
                        Description = "Allow users to add race analysis ",
                        Status = FeatureStatus.NotStarted
                    },
                    //////////////////////////////////////////////////
                    //Flex Tool
                    //////////////////////////////////////////////////
                    new AppFeature
                    {
                       Category=FeatureCategories.FlexTool,
                       Name = "Filter",
                       Description = "Allow user to filter results based on Age Group, Gender, Duration of Splits",
                       Status = FeatureStatus.Done
                    },

                    new AppFeature
                    {
                       Category=FeatureCategories.FlexTool,
                       Name = "Details",
                       Description = "Allow user to view detailed result stats for a race",
                       Status = FeatureStatus.Done
                    },
                    new AppFeature
                    {
                       Category=FeatureCategories.FlexTool,
                       Name = "Compare Races",
                       Description = "Allow user to compare race result stats",
                       Status = FeatureStatus.Done
                    },

                    new AppFeature
                    {
                        Category = FeatureCategories.FlexTool,
                        Name = "Compare Athletes",
                        Description = "Allow user to select athletes and view comparison",
                        Status = FeatureStatus.Testing
                    },

                    new AppFeature
                    {
                        Category = FeatureCategories.FlexTool,
                        Name = "Link Stats to Athletes",
                        Description = "Allow user to select stats and view the athletes in that context",
                        Status = FeatureStatus.NotStarted
                    },
                     new AppFeature
                    {
                        Category = FeatureCategories.FlexTool,
                        Name = "Race Conditions",
                        Description = "Expand race conditions",
                        Status = FeatureStatus.InProgress
                    },


                    ////////////////////////////////////////////////////////////////////
                    //hypotheticals
                    ////////////////////////////////////////////////////////////////////
                    new AppFeature
                    {
                        Category = FeatureCategories.Hypotheticals,
                        Name = "Est Finish Time",
                        Description = "Estimate range a user would finish base on their information",
                        Status =FeatureStatus.Done
                    },
                    //////////////////////////////////////////////////////////////////
                    //search
                    ////////////////////////////////////////////////////////////////////
                    new AppFeature
                    {
                        Category = FeatureCategories.Search,
                        Name = "Search for races based on conditions",
                        Description = "Search for races based on user's input : hilly,flat,cold, etc",
                        Status =FeatureStatus.NotStarted
                    },


                    //////////////////////////////////////////////////////////////////
                    //performance
                    ////////////////////////////////////////////////////////////////////
                    new AppFeature
                    {
                        Category = FeatureCategories.Performance,
                        Name = "Improve Performance",
                        Description = "Improve Performance by adding a caching layer",
                        Status =FeatureStatus.NotStarted
                    },



                };
            features.ForEach(t => context.AppFeatures.AddOrUpdate(t));
            context.SaveChanges();
        }

        private static void SeedTags(RaceAnalysisDbContext context)
        {
            context.Tags.RemoveRange(context.Tags);
            context.SaveChanges();

        }

        private static void SeedRaceConditionTag(RaceAnalysisDbContext context)
        {
            context.RaceConditionTags.RemoveRange(context.RaceConditionTags);
            context.SaveChanges();
        }

    } //class
}//namespace
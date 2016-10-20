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
            if (context.Triathletes.Count(i => i.TriathleteId > 1) == 0)
            {
                /*****************            
                            context.Races.RemoveRange(context.Races); //clear out existing rows
                            context.SaveChanges();
                            var races = new List<Race>
                            {

                                new Race
                                {
                                    RaceId=1,
                                    BaseURL="http://www.ironman.com/triathlon/events/americas/ironman/louisville/results.aspx",
                                    DisplayName="IM Louisville 10-11-2015",
                                    RaceDate = new DateTime(2015,10,11),
                                    ShortName="louisville",
                                    Distance = "140.6",
                                    Conditions = new RaceConditions {SwimGeneral="Wetsuit Legal",BikeGeneral="Rolling Hills",RunGeneral="Flat" }

                                }


                            };

                            races.ForEach(t => context.Races.AddOrUpdate(t));
                            context.SaveChanges();

            ***************************************/


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

                /***
                context.RequestContext.RemoveRange(context.RequestContext);
                context.SaveChanges();

                var contextkeys = new List<RequestContext>
                {
                    new RequestContext
                    {

                        RaceId= races.Single(i => i.DisplayName == "IM Louisville 10-11-2015").RaceId,
                        GenderId=genders.Single(i => i.DisplayName == "Male").GenderId,
                        AgeGroupId= agegroups.Single(i => i.DisplayName == "50-54").AgeGroupId
                    }

                };
                contextkeys
                    .ForEach(t => context.RequestContext.AddOrUpdate(t));
                context.SaveChanges();
                ***************************************/

                //only add one for testing if none exists....
                /********************************
                if (context.Triathletes.Count(i => i.TriathleteId > 1) == 0)
                {
                    var athletes = new List<Triathlete>
                    {
                        new Triathlete
                        {
                            RequestContextId= context.RequestContext.First().RequestContextId,
                            Link="",
                            DisplayName="Scott",
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
                ******************/
            }

        }//seed

    } //class
}//namespace
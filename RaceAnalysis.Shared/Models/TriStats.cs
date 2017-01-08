using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Script.Services;

namespace RaceAnalysis.Models
{
    public class TriStats
    {

        public TriStats()
        {

            Swim = new SwimStats();
            Bike = new BikeStats();
            Run = new RunStats();
            Finish = new FinishStats();
            DivRank = new DivRankingStats();
            GenderRank = new GenderRankingStats();
            OverallRank = new OverallRankingStats();
            Points = new PointsStats();
            Race = new Models.Race();
            Athletes = new List<Triathlete>();
        }
        public TriStats(List<Triathlete> athletes,Race race) : this()
        {
            Athletes = athletes;
            Race = race;
        }



        public Race Race { get; set; }  //each set of stats is associated with a single race, unless empty which means the athletes came from multiple races
        public List<Triathlete> Athletes{get;set;} //and a group of triathletes
     

        //STATS:
       public SwimStats              Swim { get; set; }
       public BikeStats              Bike { get; set; }
       public RunStats               Run { get; set; }
       public FinishStats            Finish { get; set; }
       public DivRankingStats        DivRank { get; set; }
       public GenderRankingStats     GenderRank { get; set; }
       public OverallRankingStats    OverallRank { get; set; }
       public PointsStats            Points { get; set; }
       public int DNFCount          { get; set; }

       public GoogleVisualizationDataTable DurationBarChartData
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                //these are the items we are comparing for each discipline
                string[] labels = { "Fastest", "Median", "Slowest" };


                // Specify the columns for the DataTable.
                dataTable.AddColumn("Stats", "string","domain");
                foreach (var l in labels)
                {
                    dataTable.AddColumn(l, "timeofday","data");
                }

                //create rows, whcih are the values for each column
                var swim = new object[]{
                        "swim",
                        new int[] { Swim.Min.Hours, Swim.Min.Minutes,Swim.Min.Seconds },
                        new int[] { Swim.Median.Hours, Swim.Median.Minutes,Swim.Median.Seconds },
                        new int[] { Swim.Max.Hours, Swim.Max.Minutes,Swim.Max.Seconds },
                    };
                   
                dataTable.AddRow(swim);

                var bike = new object[]{
                        "bike",
                        new int[] { Bike.Min.Hours, Bike.Min.Minutes,Bike.Min.Seconds },
                        new int[] { Bike.Median.Hours, Bike.Median.Minutes,Bike.Median.Seconds },
                        new int[] { Bike.Max.Hours, Bike.Max.Minutes,Bike.Max.Seconds },
                    };

                    
                dataTable.AddRow(bike);

                var run = new object[]{
                            "run",
                            new int[] { Run.Min.Hours, Run.Min.Minutes,Run.Min.Seconds },
                            new int[] { Run.Median.Hours, Run.Median.Minutes,Run.Median.Seconds },
                            new int[] { Run.Max.Hours, Run.Max.Minutes,Run.Max.Seconds },

                        };
                    
                dataTable.AddRow(run);

                var finish = new object[]{
                            "finish",
                            new int[] { Finish.Min.Hours, Finish.Min.Minutes,Finish.Min.Seconds },
                            new int[] { Finish.Median.Hours, Finish.Median.Minutes,Finish.Median.Seconds },
                            new int[] { Finish.Max.Hours, Finish.Max.Minutes,Finish.Max.Seconds },
                        };
                    
                dataTable.AddRow(finish);

                return dataTable;
            }

        }
       

    }

    public class SwimStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
        public TimeSpan StandDev { get; set; }

        public Tuple<List<Triathlete>,List<Triathlete>> FastestHalf { get; set; }
        public Tuple<List<Triathlete>, List<Triathlete>> SlowestHalf { get; set; }

        public String[] Data { get; set; }
        
    }
    public class BikeStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
        public TimeSpan StandDev { get; set; }

        public Tuple<List<Triathlete>, List<Triathlete>> FastestHalf { get; set; }
        public Tuple<List<Triathlete>, List<Triathlete>> SlowestHalf { get; set; }

        public String[] Data { get; set; }

    }
    public class RunStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
        public TimeSpan StandDev { get; set; }

        public Tuple<List<Triathlete>, List<Triathlete>> FastestHalf { get; set; }
        public Tuple<List<Triathlete>, List<Triathlete>> SlowestHalf { get; set; }
        public String[] Data { get; set; }

    }
    public class FinishStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
        public TimeSpan StandDev { get; set; }

        //note: by returning the list of athletes we will be able to display them in each group
        public Tuple<List<Triathlete>, List<Triathlete>> FastestHalf { get; set; }
        public Tuple<List<Triathlete>, List<Triathlete>> SlowestHalf { get; set; }
        public String[] Data { get; set; }

    }
    public class DivRankingStats
    {
        public double Median { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
    public class GenderRankingStats
    {
        public double Median { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
    public class OverallRankingStats
    {
        public double Median { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }
    public class PointsStats
    {
        public double Median { get; set; }
        public double Average { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }

}
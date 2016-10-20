using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public Race Race { get; set; }  //each set of stats is associated with a single race 
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

    }

    public class SwimStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
    }
    public class BikeStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
    }
    public class RunStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
    }
    public class FinishStats
    {
        public TimeSpan Median { get; set; }
        public TimeSpan Average { get; set; }
        public TimeSpan Min { get; set; }
        public TimeSpan Max { get; set; }
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
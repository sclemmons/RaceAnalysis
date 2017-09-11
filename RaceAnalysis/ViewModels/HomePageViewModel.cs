using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models 
{
    public class HomePageViewModel
    {
        public IEnumerable<Race> RecentRaces { get; set; }
        public IEnumerable<RaceAggregate> FastestRuns { get; set; }
        public IEnumerable<RaceAggregate> FastestBikes { get; set; }
        public IEnumerable<RaceAggregate> FastestSwims { get; set; }
        public IEnumerable<RaceAggregate> FastestFinishes { get; set; }

        public IEnumerable<Triathlete> FastestAthleteFinishes { get; set; }


    }
}
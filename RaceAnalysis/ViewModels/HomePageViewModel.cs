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
        public IEnumerable<RaceAggregate> FastestIMRuns { get; set; }
        public IEnumerable<RaceAggregate> FastestIMBikes { get; set; }
        public IEnumerable<RaceAggregate> FastestIMSwims { get; set; }
        public IEnumerable<RaceAggregate> FastestIMFinishes { get; set; }

        public IEnumerable<Triathlete> FastestMaleFinishesIM { get; set; }
        public IEnumerable<Triathlete> FastestFemaleFinishesIM { get; set; }


    }
}
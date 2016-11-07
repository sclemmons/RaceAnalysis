using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class AModel
    {
        public int[] selectedRaceIds { get; set; }
        public int[] selectedAgeGroupIds { get; set; }
        public int[] selectedGenderIds { get; set; }

        public string swimlowtimevalue { get; set; }
        public string swimhightimevalue { get; set; }

        public string bikelowtimevalue { get; set; }
        public string bikehightimevalue { get; set; }

        public string runlowtimevalue { get; set; }
        public string runhightimevalue { get; set; }

        public string finishlowtimevalue { get; set; }
        public string finishhightimevalue { get; set; }



    }
    public class SimpleFilterViewModel
    {
        public string Races { get; set; }
        public string AgeGroups { get; set; }
        public string Genders { get; set; }

        public string SwimLow { get; set; }
        public string SwimHigh { get; set; }
        public string BikeLow { get; set; }
        public string BikeHigh { get; set; }
        public string RunLow { get; set; }
        public string RunHigh { get; set; }

    }
}
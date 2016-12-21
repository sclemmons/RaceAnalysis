using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class SimpleRaceConditionsViewModel
    {
        public string raceId { get; set; }
        public string[] selectedSwimLayout { get; set; }

        public string[] selectedSwimMedium { get; set; }
        public string[] selectedSwimWeather { get; set; }

        public string[] selectedSwimOther { get; set; }
        
        public string[] selectedBikeLayout { get; set; }

        public string[] selectedBikeMedium { get; set; }

        public string[] selectedBikeWeather { get; set; }

        public string[] selectedBikeOther { get; set; }
        public string[] selectedRunLayout { get; set; }
        public string[] selectedRunMedium { get; set; }
        public string[] selectedRunWeather { get; set; }
        public string[] selectedRunOther { get; set; }

    }
}
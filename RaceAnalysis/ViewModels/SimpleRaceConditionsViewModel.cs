using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class SimpleRaceConditionsViewModel
    {
        public string raceId { get; set; }
       public List<string> selectedSwimLayout { get; set; }

       public List<string> selectedSwimMedium { get; set; }
       public List<string> selectedSwimWeather { get; set; }

       public List<string> selectedSwimOther { get; set; }
        
       public List<string> selectedBikeLayout { get; set; }

       public List<string> selectedBikeMedium { get; set; }

       public List<string> selectedBikeWeather { get; set; }

       public List<string> selectedBikeOther { get; set; }
       public List<string> selectedRunLayout { get; set; }
       public List<string> selectedRunMedium { get; set; }
       public List<string> selectedRunWeather { get; set; }
       public List<string> selectedRunOther { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{

    public class TriStatsViewModel
    {
        public TriStatsViewModel()
        {
            //to prevent nulls
            Filter = new RaceFilterViewModel();
            Stats = new List<TriStats>();
        }
        public RaceFilterViewModel Filter { get; set; }
        public List<TriStats> Stats { get; set; }
     

    }
}

    
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.ServiceSupport
{
    public class BasicRaceCriteria : IRaceCriteria
    {
        public BasicRaceCriteria()
        {

            SelectedRaceIds = new List<int>();
            SelectedAgeGroupIds = new List<int>();
            SelectedGenderIds = new List<int>();
        }
        public IList<int> SelectedAgeGroupIds { get; set; }
        
        public IList<int> SelectedGenderIds { get; set; }
        
        public IList<int> SelectedRaceIds { get; set; }
        
    }
}
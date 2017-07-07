using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RaceAnalysis.ServiceSupport
{
    public class BasicRaceCriteria : IRaceCriteria
    {
        public BasicRaceCriteria()
        {

            SelectedRaceIds = new List<string>();
            SelectedAgeGroupIds = new List<int>();
            SelectedGenderIds = new List<int>();
        }
        public IList<int> SelectedAgeGroupIds { get; set; }
        
        public IList<int> SelectedGenderIds { get; set; }
        
        public IList<string> SelectedRaceIds { get; set; }
        
        public override String ToString()
        {
            var sb = new StringBuilder();
            foreach( var id in SelectedRaceIds)
            {
                sb.AppendFormat("{0},", id);
            }
            foreach(var id in SelectedAgeGroupIds)
            {
               sb.AppendFormat("{0},", id);
            }
            foreach(var id in SelectedGenderIds)
            {
                sb.AppendFormat("{0},", id);
            }

            return sb.ToString();
        }
    }


   public class BasicRaceCriteriaDiagnostics : IRaceCriteria
    {
        private IRaceCriteria _RaceCriteria;
        public BasicRaceCriteriaDiagnostics(IRaceCriteria raceCriteria)
        {
            _RaceCriteria = raceCriteria;
        }

        public IList<int> SelectedAgeGroupIds
        {
            get
            {
                return _RaceCriteria.SelectedAgeGroupIds;
            }

            set
            {
                _RaceCriteria.SelectedAgeGroupIds = value;
            }
        }

        public IList<int> SelectedGenderIds
        {
            get
            {
                return _RaceCriteria.SelectedGenderIds;
            }

            set
            {
                _RaceCriteria.SelectedGenderIds = value;
            }
        }

        public IList<string> SelectedRaceIds
        {
            get
            {
                return _RaceCriteria.SelectedRaceIds;
            }

            set
            {
                _RaceCriteria.SelectedRaceIds = value;
            }
        }
    }
}
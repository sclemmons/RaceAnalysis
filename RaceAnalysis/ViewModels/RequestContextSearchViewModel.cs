using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class RequestContextSearchViewModel : BaseViewModel
    {

        public List<Race> Races { get; set; }
      
        private List<string> _selectedRaces = new List<string>();
        public List<string> SelectedRaces
        {
            get
            {

                return _selectedRaces;
            }
            set
            {
                _selectedRaces = value;
            }
        }
               
    }
}
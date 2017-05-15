using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace RaceAnalysis.Models
{
    public class TriathletesViewModel :BaseViewModel
    {
        public int TotalCount { get; set; }

        public int SelectedAthleteId { get; set; }

       
        public IEnumerable<ShallowTriathlete> Athletes { get; set; }
    }
}
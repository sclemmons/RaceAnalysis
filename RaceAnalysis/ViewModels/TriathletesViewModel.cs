using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using System.Collections;

namespace RaceAnalysis.Models
{
    public class TriathletesViewModel :BaseViewModel
    {
        public int TotalCount { get; set; }

        public int SelectedAthleteId { get; set; }
        public string Name { get { return "Name"; } }
    }
}
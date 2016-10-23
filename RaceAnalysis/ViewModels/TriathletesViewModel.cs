using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using System.Collections;

namespace RaceAnalysis.Models
{
    public class TriathletesViewModel
    {
        public RaceFilterViewModel Filter { get; set; }

        public IPagedList<Triathlete> Triathletes { get; set; }

    }
}
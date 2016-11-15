using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using MoreLinq;


namespace RaceAnalysis.Helpers
{
    //basically creating a new class for this so I can try out a different linq extension library
    public class TriStatsCalculatorExtended : TriStatsCalculator
    {
        public TriStatsCalculatorExtended(List<Triathlete> athletes) : base(athletes){}
    }
}
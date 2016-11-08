using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.ServiceSupport
{
    public class BasicDurationFilter : IDurationFilter
    {
        public TimeSpan SwimLow { get; set; }
        public TimeSpan SwimHigh { get; set; }
        public TimeSpan BikeLow { get; set; }
        public TimeSpan BikeHigh { get; set; }
        public TimeSpan RunLow { get; set; }
        public TimeSpan RunHigh { get; set; }
        public TimeSpan FinishLow { get; set; }
        public TimeSpan FinishHigh { get; set; }

    }
}

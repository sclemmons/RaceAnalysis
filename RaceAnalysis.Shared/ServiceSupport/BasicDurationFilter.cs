using RaceAnalysis.Service.Interfaces;
using System;


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

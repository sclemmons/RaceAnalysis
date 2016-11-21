using RaceAnalysis.Service.Interfaces;
using System;


namespace RaceAnalysis.ServiceSupport
{
    public class BasicDurationFilter : IDurationFilter
    {
        public BasicDurationFilter()
        { //by default, set these to min and max in order to get all. 
            //in most cases the has selected values from a filter but there
            //are cases where these may be overriden
            SwimLow     = TimeSpan.MinValue;
            SwimHigh    = TimeSpan.MaxValue;
            BikeLow     = TimeSpan.MinValue;
            BikeHigh    = TimeSpan.MaxValue;
            RunLow      = TimeSpan.MinValue;
            RunHigh     = TimeSpan.MaxValue;
            FinishLow   = TimeSpan.MinValue;
            FinishHigh  = TimeSpan.MaxValue;
        }
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

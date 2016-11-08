using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Service.Interfaces
{
    public interface IDurationFilter  
    {
        TimeSpan SwimLow { get; set; }
        TimeSpan SwimHigh { get; set; }
        TimeSpan BikeLow { get; set; }
        TimeSpan BikeHigh { get; set; }
        TimeSpan RunLow { get; set; }
        TimeSpan RunHigh { get; set; }
        TimeSpan FinishLow { get; set; }
        TimeSpan FinishHigh { get; set; }
    }
}

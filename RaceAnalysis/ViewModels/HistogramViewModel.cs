using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class HistogramViewModel 
    {
        public HistogramViewModel()
        {
            //to prevent nulls
            Filter = new RaceFilterViewModel();
           
        }

        public RaceFilterViewModel Filter { get; set; }
        public List<Triathlete> Triathletes { get; set; }
        public List<TriStats> Stats { get; set; }
        public TimeSpan FinishMedian { get; set; }


        public List<object> ChartData
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] {"Hrs" });

                foreach (var t in Triathletes)
                {

                    /*the following would be the ideal way to do this but the histogram chart will not display the hAxis in the 
                     * timeofday format. It sees it as a number. The toolip displays the time correctly. 
                     * After spending many hours on this I'm opting to display it as a number.
                     * *************************************************************************/
                    //list.Add(new object[] { t.Name, new int[] { t.Finish.Hours, t.Finish.Minutes, t.Finish.Seconds } });

                    if(t.Finish.TotalHours > 0)
                        list.Add(new object[] {Math.Round(t.Finish.TotalHours,2)}
                    
                    );

                }

                return list;
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class HypotheticalsViewModel : TriStatsViewModel
    {
        public HypotheticalsViewModel()
        {
            PopulateSkillLevels();
        }

        public IList<SkillLevel> AvailableSkillLevels { get; set; }
        public string SelectedSkillLevel { get; set; }

        public string SelectedSplit {get;set;} //run,swim,bike   this is a way that we can indicate to generic views what split we are showing
        public void SaveSkillLevel(string value)
        {
            SelectedSkillLevel = value;
        }
        
        public List<object> FinishData
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] {
                    AvailableSkillLevels.Where(s => s.Value == "1").Select(s => s.DisplayName).First(),
                    AvailableSkillLevels.Where(s => s.Value == "2").Select(s => s.DisplayName).First(),
                    AvailableSkillLevels.Where(s => s.Value == "3").Select(s => s.DisplayName).First(),
                    AvailableSkillLevels.Where(s => s.Value == "4").Select(s => s.DisplayName).First()
                });

                foreach (var t in Triathletes)
                {

                    /*the following would be the ideal way to do this but the histogram chart will not display the hAxis in the 
                     * timeofday format. It sees it as a number. The toolip displays the time correctly. 
                     * After spending many hours on this I'm opting to display it as a number.
                     * *************************************************************************/
                    //list.Add(new object[] { t.Name, new int[] { t.Finish.Hours, t.Finish.Minutes, t.Finish.Seconds } });


                    if (t.Finish.TotalHours > 0)
                    {

                        var hrs = Math.Round(t.Finish.TotalHours, 2);

                        var q1 = Stats[0].Finish.FastestHalf.Item1.Max(x => x.Finish.TotalHours);
                        var q2 = Stats[0].Finish.FastestHalf.Item2.Max(x => x.Finish.TotalHours);
                        var q3 = Stats[0].Finish.SlowestHalf.Item1.Max(x => x.Finish.TotalHours);
                        var q4 = Stats[0].Finish.SlowestHalf.Item2.Max(x => x.Finish.TotalHours);

                        list.Add(new double?[] {
                                    hrs <= q1 ? hrs :(double?) null, //Q1
                                    (hrs > q1 && hrs <= q2)  ? hrs :(double?) null, //Q2
                                    (hrs > q2 && hrs <= q3) ? hrs :(double?) null, //Q3
                                    (hrs > q3 && hrs <= q4) ? hrs :(double?) null, //Q4
                        });

                    }
                }

                return list;
            }

        }



        #region Private Region

        private void PopulateSkillLevels()
        {
            var list = new List<SkillLevel>();

            //skill level values correspond to Quarters, so don't change! 
            list.Add(new SkillLevel(display: "Back of Pack", value: "4"));
            list.Add(new SkillLevel(display: "Mid Pack", value: "3"));
            list.Add(new SkillLevel(display: "Faster than most", value: "2"));
            list.Add(new SkillLevel(display: "Leader of the Pack!", value: "1"));

            AvailableSkillLevels = list;
        }
        #endregion

    }

    public class SkillLevel
    {
        public SkillLevel(string display,string value)
        {
            Value = value;
            DisplayName = display;
        }
        public string Value { get; set; }
        public string DisplayName { get; set; }
    }

   
}
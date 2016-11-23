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

        public IList<SkillLevel> AvailableSkillLevels{ get; set; }
        public string SelectedSkillLevel { get; set; }


        private void PopulateSkillLevels()
        {
            var list = new List<SkillLevel>();
            
            //skill level values correspond to Quarters, so don't change! 
            list.Add(new SkillLevel( display:"Back of Pack", value:"4" ));
            list.Add(new SkillLevel(display: "Mid Pack", value: "3"));
            list.Add(new SkillLevel(display: "Faster than most", value: "2"));
            list.Add(new SkillLevel(display: "Leader of the Pack!", value: "1"));

            AvailableSkillLevels = list;
        }
        public void SaveSkillLevel(string value)
        {
            SelectedSkillLevel = value;
        } 
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Models
{
    public class AppFeature
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppFeatureId { get; set; }

        public FeatureCategories Category { get; set; }  //enum
        public string Name { get; set; }
        public string Description { get; set; }

        public FeatureState State { get; set; } //enum

        public int VoteCount { get; set; }

    }
    public enum FeatureCategories
    {
        Admin, 
        FlexTool,
        Hypotheticals,
        Search,
        Performance,
        ContentContrib
    }
    public enum FeatureState
    {
        NotStarted,
        InProgress,
        Testing,
        Done
    }

}

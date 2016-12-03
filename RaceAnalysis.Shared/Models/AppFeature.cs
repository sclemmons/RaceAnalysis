using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public FeatureStatus Status { get; set; } //enum

        [Display(Name = "Vote Count")]
        public int VoteCount { get; set; }

    }
    public enum FeatureCategories
    {
        None=0,
        Admin, 
        FlexTool,
        Hypotheticals,
        Search,
        Performance,

        [Display(Name="Content")]
        ContentContrib
    }
    public enum FeatureStatus
    {
        NotStarted,
        InProgress,
        Testing,
        Done
    }

}

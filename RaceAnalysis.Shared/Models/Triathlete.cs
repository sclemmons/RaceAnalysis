using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Nest;

namespace RaceAnalysis.Models
{
    [ElasticsearchType(IdProperty=nameof(TriathleteId))]
    public class Triathlete
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TriathleteId { get; set; }

    
        public string Link { get; set; }
    
        public string Name { get; set; }
        public string Country { get; set; }

        [Display(Name = "Division\n Rank")]
        public int DivRank { get; set; }

        [Display(Name = "Gender\n Rank")]
        public int GenderRank { get; set; }

        [Display(Name = "Overall\n Rank")]
        public int OverallRank { get; set; }

        public TimeSpan Swim { get; set; }
        public TimeSpan Bike { get; set; }
        public TimeSpan Run { get; set; }
        public TimeSpan Finish { get; set; }
        public int Points { get; set; }

        [ForeignKey("RequestContext")]
        public int RequestContextId { get; set; }

        [Nested(IncludeInParent =true)]
        public virtual RequestContext RequestContext {get;set;}
              
    
     //  [Nested(IncludeInParent =true)]
       [JsonIgnore]
        public virtual Race Race
        {
            get { return RequestContext.Race; }
        }
     

    }

   
}
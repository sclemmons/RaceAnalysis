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
            get {
                return RequestContext.Race; 
            }
        }
     
        [NotMapped]
        public bool IsSelected { get; set; }


        public static string FormatName(string name)
        {
            string[] s = name.Split(',');

            if (s.Length > 1) //the given name contained commas
                return string.Format("{0}, {1}", s[0].Trim(), s[1].Trim());
            else //just return the orinal without spaces
                return name.Trim();

        }
    }



    public class TriathleteComparer : EqualityComparer<Triathlete>
    {
        public override bool Equals(Triathlete x, Triathlete y)
        {


            string xstr = string.Join("", x.Name.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            string ystr = string.Join("", y.Name.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));


            return xstr.Equals(ystr);
        }

        public override int GetHashCode(Triathlete obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class ShallowTriathlete
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string RaceId { get; set; }
    }


    public class ShallowTriathleteComparer : EqualityComparer<ShallowTriathlete>
    {
        public override bool Equals(ShallowTriathlete x, ShallowTriathlete y)
        {
            return x.Name.Equals(y.Name);
        }

        public override int GetHashCode(ShallowTriathlete obj)
        {
            return obj.Name.GetHashCode(); 
       }
    } 
}
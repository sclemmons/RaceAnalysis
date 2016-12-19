using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Models
{
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        public TagType Type{get;set;}
        public string Value { get; set; }
    }

    public enum TagType
    {
        SwimLayout,
        SwimMedium,
        SwimWeather,
        SwimOther,
        BikeLayout,
        BikeMedium,
        BikeWeather,
        RunLayout,
        RunMedium,
        RunWeather,
        RunOther
    }

}

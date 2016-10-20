using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceAnalysis.Models
{
    public class RaceConditions
    {
        public RaceConditions()
        {
            SwimGeneral = String.Empty;
            BikeGeneral = String.Empty;
            RunGeneral = String.Empty;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceConditionsId { get; set; }
           
        //place holders right now, intend to get more specific info
        [Display(Name = "Swim General")]
        public string SwimGeneral { get; set; }

        [Display(Name = "Bike General")]
        public string BikeGeneral { get; set; }

        [Display(Name = "Run General")]
        public string RunGeneral { get; set; }
                

    }
}
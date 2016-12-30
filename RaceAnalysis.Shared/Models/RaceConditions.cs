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
            SwimLayout = new List<RaceConditionTag>();
            SwimMedium = new List<RaceConditionTag>();
            SwimWeather = new List<RaceConditionTag>();
            SwimOther = new List<RaceConditionTag>();


            BikeLayout = new List<RaceConditionTag>();
            BikeMedium = new List<RaceConditionTag>();
            BikeWeather = new List<RaceConditionTag>();
            BikeOther = new List<RaceConditionTag>();

            RunLayout = new List<RaceConditionTag>();
            RunMedium = new List<RaceConditionTag>();
            RunWeather = new List<RaceConditionTag>();
            RunOther = new List<RaceConditionTag>();

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceConditionsId { get; set; }

        public virtual List<Tag> Tags { get; set; }
         
        [Display(Name = "Swim Layout")]
        public virtual List<RaceConditionTag> SwimLayout { get; set; }

        [Display(Name = "Swim Medium")]
        public virtual List<RaceConditionTag> SwimMedium { get; set; }

        [Display(Name = "Swim Weather")]
        public virtual List<RaceConditionTag> SwimWeather { get; set; }

        [Display(Name = "Swim Other")]
        public virtual List<RaceConditionTag> SwimOther { get; set; }

        

        [Display(Name = "Bike Layout")]
        public virtual List<RaceConditionTag> BikeLayout { get; set; }

        [Display(Name = "Bike Medium")]
        public virtual List<RaceConditionTag> BikeMedium { get; set; }

        [Display(Name = "Bike Weather")]
        public virtual List<RaceConditionTag> BikeWeather { get; set; }

        [Display(Name = "Bike Other")]
        public virtual List<RaceConditionTag> BikeOther { get; set; }




        [Display(Name = "Run Layout")]
        public virtual List<RaceConditionTag> RunLayout { get; set; }

        [Display(Name = "Run Medium")]
        public virtual List<RaceConditionTag> RunMedium { get; set; }

        [Display(Name = "Run Weather")]
        public virtual List<RaceConditionTag> RunWeather { get; set; }

        [Display(Name = "Run Other")]
        public virtual List<RaceConditionTag> RunOther { get; set; }



    }
}
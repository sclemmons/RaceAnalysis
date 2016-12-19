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
            SwimLayout = String.Empty;
            BikeLayout = String.Empty;
            RunLayout = String.Empty;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceConditionsId { get; set; }
           
        [Display(Name = "Swim Layout")]
        public string SwimLayout { get; set; }

        [Display(Name = "Swim Medium")]
        public string SwimMedium { get; set; }

        [Display(Name = "Swim Weather")]
        public string SwimWeather { get; set; }

        [Display(Name = "Swim Other")]
        public string SwimOther { get; set; }





        [Display(Name = "Bike Layout")]
        public string BikeLayout { get; set; }

        [Display(Name = "Bike Medium")]
        public string BikeMedium { get; set; }

        [Display(Name = "Bike Weather")]
        public string BikeWeather { get; set; }

        [Display(Name = "Bike Other")]
        public string BikeOther { get; set; }




        [Display(Name = "Run Layout")]
        public string RunLayout { get; set; }

        [Display(Name = "Run Medium")]
        public string RunMedium { get; set; }

        [Display(Name = "Run Weather")]
        public string RunWeather { get; set; }

        [Display(Name = "Run Other")]
        public string RunOther { get; set; }



    }
}
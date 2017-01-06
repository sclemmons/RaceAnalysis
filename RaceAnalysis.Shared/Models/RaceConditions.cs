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

        }
        public int RaceConditionsId { get; set; }

        public virtual List<RaceConditionTag> RaceConditionTags { get; set; }


        [NotMapped]
        [Display(Name = "Swim Layout")]
        public List<RaceConditionTag> SwimLayout
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.SwimLayout).ToList();
            }

        }

        [NotMapped]
        [Display(Name = "Swim Medium")]
        public List<RaceConditionTag> SwimMedium
        { 
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.SwimMedium).ToList();
            }
        }


        [NotMapped]
        [Display(Name = "Swim Weather")]
        public List<RaceConditionTag> SwimWeather 
        { 
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.SwimWeather).ToList();
            }
        }

        [NotMapped]
        [Display(Name = "Swim Other")]
        public List<RaceConditionTag> SwimOther
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.SwimOther).ToList();
            }
        }

        [NotMapped]
        [Display(Name = "Bike Layout")]
        public List<RaceConditionTag> BikeLayout
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.BikeLayout).ToList();
            }
        }

        [NotMapped]
        [Display(Name = "Bike Medium")]
        public  List<RaceConditionTag> BikeMedium
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.BikeMedium).ToList();
            }
        }
        [NotMapped]
        [Display(Name = "Bike Weather")]
        public  List<RaceConditionTag> BikeWeather
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.BikeWeather).ToList();
            }
        }
        [NotMapped]
        [Display(Name = "Bike Other")]
        public  List<RaceConditionTag> BikeOther
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.BikeOther).ToList();
            }
        }


        [NotMapped]
        [Display(Name = "Run Layout")]
        public  List<RaceConditionTag> RunLayout
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.RunLayout).ToList();
            }
        }

        [NotMapped]
        [Display(Name = "Run Medium")]
        public  List<RaceConditionTag> RunMedium
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.RunMedium).ToList();
            }
        }

        [NotMapped]
        [Display(Name = "Run Weather")]
        public  List<RaceConditionTag> RunWeather
        {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.RunWeather).ToList();
            }
        }


        [NotMapped]
        [Display(Name = "Run Other")]
        public  List<RaceConditionTag> RunOther
         {
            get
            {
                return RaceConditionTags.Where(rc => rc.Tag.Type == TagType.RunOther).ToList();
            }
        }


    }
}
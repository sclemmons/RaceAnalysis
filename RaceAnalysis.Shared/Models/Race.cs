using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Nest;
using Newtonsoft.Json;

namespace RaceAnalysis.Models
{
    [ElasticsearchType(IdProperty = nameof(RaceId))]
    public class Race
    {
        public Race() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string RaceId { get; set; }
        

        public string BaseURL { get; set; } //this is for the REST service to get the data
    
        public string RaceURL { get; set; } //this is the URL used for users to go to view the race 

        [Display(Name = "Short Display Name")]
        public string DisplayName { get; set; } //this is displayed through the analysis

        [Display(Name = "Long Display Name")]
        public string LongDisplayName { get; set; } //this is displayed on the full race list page

        [Display(Name = "Race Date")]
        [MinDate("01/01/2006")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] /*date format needs to be like this to conform to standard. However the display should render like we want it*/
        public DateTime RaceDate { get; set; }

        [Display(Name = "Short Name")]
        public string ShortName { get; set; }//this is used for the REST call to get the data

        [NotMapped]
        public string RaceCategoryName {
            get { return LongDisplayName.Substring(0, LongDisplayName.LastIndexOf(" ")); }
        }

        public string Distance { get; set; } //this is for display and grouping purposes

        [ForeignKey("Conditions")]
        public int ConditionsId { get; set; }
        //     [JsonIgnore]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual RaceConditions Conditions { get; set; }

        public string ApiName { get; set; } //at this point, if this is null, we assume it is IronmanClient

        public string ValidateMessage { get; set; }

        public bool IsAggregated { get; set; }
    }



    public class MinDateAttribute : ValidationAttribute
    {
        private const string DateFormat = "MM/dd/yyyy";
        private const string DefaultErrorMessage =
     "'{0}' must be a date greater than {1:d}";

        public DateTime MinDate { get; set; }


        public MinDateAttribute(string minDate)
            : base(DefaultErrorMessage)
        {
            MinDate = ParseDate(minDate);
        }

        public override bool IsValid(object value)
        {
            if (value == null || !(value is DateTime))
            {
                return true;
            }
            DateTime dateValue = (DateTime)value;
            return MinDate <= dateValue;
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinDate);
        }

        private static DateTime ParseDate(string dateValue)
        {
            return DateTime.ParseExact(dateValue, DateFormat, CultureInfo.InvariantCulture);
        }


    }
  

    public class ShallowRace
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    public class ShallowRaceComparer : EqualityComparer<ShallowRace>
    {
        public override bool Equals(ShallowRace x, ShallowRace y)
        {
            return x.Name.Equals(y.Name);
        }

        public override int GetHashCode(ShallowRace obj)
        {
            return obj.Name.GetHashCode();
        }
    }

}

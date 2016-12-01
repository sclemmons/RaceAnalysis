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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RaceId { get; set; }

        public string BaseURL { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Race Date")]
        [MinDate("01/01/2006")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] /*date format needs to be like this to conform to standard. However the display should render like we want it*/
        public DateTime RaceDate { get; set; }

        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        public string Distance { get; set; }

        [ForeignKey("Conditions")]
        public int ConditionsId { get; set; }
        //     [JsonIgnore]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual RaceConditions Conditions { get; set; }
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


}

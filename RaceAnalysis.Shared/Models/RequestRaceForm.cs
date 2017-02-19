using System;
using System.ComponentModel.DataAnnotations;

namespace RaceAnalysis.Models
{
    public class RequestRaceForm
    {
        [Required(ErrorMessage = "Please provide your name.")]
        [Display(Name ="Race Name")]
        public string RaceName { get; set; }

        [Required(ErrorMessage = "Please provide the URL to the race's website.")]
        public string URL { get; set; }

        [Required(ErrorMessage = "Please provide your email address."), EmailAddress]
        public string Email { get; set; }

        public string Message { get; set; }
    }
}

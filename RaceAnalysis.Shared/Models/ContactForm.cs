using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RaceAnalysis.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "You must provide your name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide your email."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must include a message.")]
        public string Message { get; set; }
    }
}

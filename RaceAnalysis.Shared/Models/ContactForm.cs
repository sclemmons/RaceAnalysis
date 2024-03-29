﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RaceAnalysis.Models
{
    public class ContactForm
    {
        [Required(ErrorMessage = "Please provide your name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide your email address."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please include a message.")]
        public string Message { get; set; }
    }
}

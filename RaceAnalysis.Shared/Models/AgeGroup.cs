using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceAnalysis.Models
{
    public class AgeGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AgeGroupId { get; set; }

        public string Value { get; set; }
        public string DisplayName { get; set; }
    }
}
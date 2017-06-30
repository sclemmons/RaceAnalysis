using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Models
{
    public class AthleteCache
    {
        [Key]
        public int AthleteId { get; set; }
        public bool IsCached { get; set; }

    }
}

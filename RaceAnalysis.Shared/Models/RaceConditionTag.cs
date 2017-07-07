using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Models
{
    public class RaceConditionTag
    {
        [Key, Column(Order = 0)]
        public int RaceConditionsId { get; set; }
        [Key, Column(Order = 1)]
        public int TagId { get; set; }

        public virtual RaceConditions RaceConditions { get; set; }

        public virtual Tag Tag { get; set; }

        public int Count { get; set; }
    }
}

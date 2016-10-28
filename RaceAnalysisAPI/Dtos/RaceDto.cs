using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysisAPI.Dtos
{
    public class RaceDto
    {
        public int RaceId { get; set; }

        public string BaseURL { get; set; }

        public string NewDisplayName { get; set; }

        public DateTime RaceDate { get; set; }

        public string ShortName { get; set; }

        public string Distance { get; set; }

        public int ConditionsId { get; set; }
     }



}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Models
{

    public class AgeGroupAggregate
    {

        [Key, Column(Order = 0)]
        [ForeignKey("Race")]
        public string RaceId { get; set; }
        public Race Race { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey( "AgeGroup")]
        public int AgeGroupId { get; set; }
        public AgeGroup AgeGroup { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        public int AthleteCount { get; set; }
        public int DNFCount { get; set; }
        public int MaleCount { get; set; }
        public int FemaleCount { get; set; }

        public TimeSpan SwimMedian { get; set; }
        public TimeSpan BikeMedian { get; set; }
        public TimeSpan RunMedian { get; set; }
        public TimeSpan FinishMedian { get; set; }

        public TimeSpan SwimFastest { get; set; }
        public TimeSpan BikeFastest { get; set; }
        public TimeSpan RunFastest { get; set; }
        public TimeSpan FinishFastest { get; set; }

        public TimeSpan SwimSlowest { get; set; }
        public TimeSpan BikeSlowest { get; set; }
        public TimeSpan RunSlowest { get; set; }
        public TimeSpan FinishSlowest { get; set; }

        public TimeSpan SwimStdDev { get; set; }
        public TimeSpan BikeStdDev { get; set; }
        public TimeSpan RunStdDev { get; set; }
        public TimeSpan FinishStdDev { get; set; }

    }



}

    
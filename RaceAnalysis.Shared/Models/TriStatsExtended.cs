using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace RaceAnalysis.Models
{
    public class TriStatsExtended : TriStats
    {
        private List<Triathlete> _Triathletes; 
        public TriStatsExtended (IEnumerable<Triathlete> athletes)
        {
            _Triathletes = athletes.ToList();
        }
        public Triathlete FastestSwimmer
        {
            get
            {
                var athlete = _Triathletes.MinBy(t => t.Swim);
                return athlete;
            }

        }

        public Triathlete SlowestSwimmer
        {
            get
            {
                var athlete = _Triathletes.MaxBy(t => t.Swim);
                return athlete;
            }
        }
        public Triathlete FastestBiker
        {
            get
            {
                var athlete = _Triathletes.MinBy(t => t.Bike);
                return athlete;
            }

        }

        public Triathlete SlowestBiker
        {
            get
            {
                var athlete = _Triathletes.MaxBy(t => t.Bike);
                return athlete;
            }
        }
        public Triathlete FastestRunner
        {
            get
            {
                var athlete = _Triathletes.MinBy(t => t.Run);
                return athlete;
            }

        }

        public Triathlete SlowestRunner
        {
            get
            {
                var athlete = _Triathletes.MaxBy(t => t.Run);
                return athlete;
            }
        }
        public Triathlete FastestFinisher
        {
            get
            {
                var athlete = _Triathletes.MinBy(t => t.Finish);
                return athlete;
            }

        }

        public Triathlete SlowestFinisher
        {
            get
            {
                var athlete = _Triathletes.MaxBy(t => t.Finish);
                return athlete;
            }
        }

    }
}

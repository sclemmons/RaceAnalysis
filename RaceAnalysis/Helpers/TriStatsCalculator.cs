using System;
using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using System.Linq.Dynamic;
using RaceAnalysis.Models;
using RaceAnalysis.Helpers;
using MoreLinq;

namespace RaceAnalysis.Helpers
{
    public class TriStatsCalculator
    {
        protected List<Triathlete> _Triathletes;
        public TriStatsCalculator(List<Triathlete> athletes)
        {
            _Triathletes = athletes;
        }

        public int NumberDNFs()
        {
            return _Triathletes.Count(t => t.Finish.TotalSeconds == 0);

        }
        public TimeSpan TimeSpanHistogram(string timeSpanProperty)
        {
            var query = _Triathletes.AsQueryable()
            .Where(timeSpanProperty + ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                .Select(timeSpanProperty);

            var list = query.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                var bins =
                    list.Histogram(20, t => 
                        {
                            return Convert.ToInt32(t.TotalSeconds);
                        });
            }
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));

            return ts;
        }
        
        public TimeSpan SigmaPlusOne(TimeSpan median, TimeSpan stdDev)
        { //SigmaPlusOne and SigmaMinusOne together represent 68.27% of the population 
            return median + stdDev; 
        }

        public TimeSpan SigmaMinusOne(TimeSpan median, TimeSpan stdDev)
        {//SigmaPlusOne and SigmaMinusOne together represent 68.27% of the population 

               return median - stdDev;
        }
             
        public static Tuple<List<Triathlete>,List<Triathlete>> Split(List<Triathlete> athletes, string timeSpanProperty)
        {
            var q0 = athletes.AsQueryable()
            .Where(timeSpanProperty + ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                .Select(timeSpanProperty);

            var list = q0.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                seconds = list.Median(t =>
                {
                    return Convert.ToInt32(t.TotalSeconds);
                });
            }
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds)); //median time for this list

            //get the athletes below and above the median
            var bottomHalf = athletes.AsQueryable()
                .Where(timeSpanProperty + ".TotalSeconds <=" + ts.TotalSeconds).ToList();
            var topHalf = athletes.AsQueryable()
                .Where(timeSpanProperty + ".TotalSeconds >" + ts.TotalSeconds).ToList();
                       

            return Tuple.Create(bottomHalf,topHalf);
        }


        public TimeSpan SecondQuarter(TimeSpan median, TimeSpan stdDev)
        {
            var ticks = median.Ticks - (stdDev.Ticks * .6745);
            return new TimeSpan((long)ticks).RoundToNearest(TimeSpan.FromSeconds(1));
            
        }
        public TimeSpan ThirdQuarter(TimeSpan median, TimeSpan stdDev)
        {
            var ticks = median.Ticks + (stdDev.Ticks * .6745);
            return new TimeSpan((long)ticks).RoundToNearest(TimeSpan.FromSeconds(1));

        }
        public TimeSpan FourthQuarter(TimeSpan median, TimeSpan stdDev)
        {
            var ticks = median.Ticks + (stdDev.Ticks * 2.698);
            return new TimeSpan((long)ticks).RoundToNearest(TimeSpan.FromSeconds(1));

        }


        public TimeSpan TimeSpanStandardDeviation(string timeSpanProperty)
        {
            var query = _Triathletes.AsQueryable()
            .Where(timeSpanProperty + ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                .Select(timeSpanProperty);

            var list = query.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                seconds = list.StandardDeviation(t =>
                {
                    return Convert.ToInt32(t.TotalSeconds);
                });
            }
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));

            return ts;

        }

        public TimeSpan TimeSpanMedian(string timeSpanProperty)
        {
            var query = _Triathletes.AsQueryable()
            .Where(timeSpanProperty + ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                .Select(timeSpanProperty);

            var list = query.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                seconds = list.Median(t =>
               {
                   return Convert.ToInt32(t.TotalSeconds);
               });
            }
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));

            return ts;
        }
        public TimeSpan TimeSpanAverage(string timeSpanProperty)
        {
             var query = _Triathletes.AsQueryable()
                 .Where(timeSpanProperty + ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                 .Select(timeSpanProperty);

            var list = query.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                seconds = list.Average<dynamic>(t =>
                {
                    return Convert.ToInt32(t.TotalSeconds);
                });
            }


            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));

            return ts;
        }
        public TimeSpan TimeSpanMin(string timeSpanProperty)
        {

            var query = _Triathletes.AsQueryable()
                  .Where(timeSpanProperty+ ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                  .Select(timeSpanProperty);

            var list = query.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                seconds = list.Min<dynamic>(t =>
                {
                    return Convert.ToInt32(t.TotalSeconds);
                });
            }

            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));

            return ts;
        }
        public TimeSpan TimeSpanMax(string timeSpanProperty)
        {

            var query = _Triathletes.AsQueryable()
                .Where(timeSpanProperty + ".TotalSeconds > TimeSpan(0, 0, 0).TotalSeconds")
                 .Select(timeSpanProperty);

            var list = query.Cast<dynamic>().ToList();

            double seconds = 0;
            if (list.Count > 0)
            {
                seconds = list.Max<dynamic>(t =>
                {
                    return Convert.ToInt32(t.TotalSeconds);
                });
            }

            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));

            return ts;
        }
        public double IntMedian(string intProperty)
        {
            var listOfInts = _Triathletes.AsQueryable()
                .Where(intProperty + " > 0")
                .Select(intProperty).Cast<int>().ToList();

            double calc = 0;
            if (listOfInts.Count > 0)
             calc = listOfInts.Median();

            return Math.Round(calc, 2);
        }
        public double IntAverage(string intProperty)
        {
            var listOfInts = _Triathletes.AsQueryable()
                  .Where(intProperty + " > 0")
                  .Select(intProperty).Cast<int>().ToList();

            double calc = 0;
            if (listOfInts.Count > 0)
                calc = listOfInts.Average();

            return Math.Round(calc, 2);
        }
        public double IntMin(string intProperty)
        {
            var listOfInts = _Triathletes.AsQueryable()
              .Where(intProperty + " > 0")
                 .Select(intProperty).Cast<int>().ToList();

            double calc = 0;
            if (listOfInts.Count > 0)
                calc = listOfInts.Min();

            return Math.Round(calc, 2);
        }
        public double IntMax(string intProperty)
        {
            var listOfInts = _Triathletes.AsQueryable()
                    .Where(intProperty + " > 0")
                    .Select(intProperty).Cast<int>().ToList();

            double calc = 0;
            if (listOfInts.Count > 0)
                calc = listOfInts.Max();

            return Math.Round(calc, 2);
        }


        
    }
}
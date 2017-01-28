using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using X.PagedList;
using RaceAnalysis.Service.Interfaces;
using System.Linq;

namespace RaceAnalysis.Controllers
{
    public abstract class BaseController : Controller
    {
        protected RaceAnalysisDbContext _DBContext = new RaceAnalysisDbContext();
        protected IRaceService _RaceService;

        public BaseController(IRaceService service)
        {
            _RaceService = service;
          
        }
        public ActionResult ResetDurations()
        {
            var model = new RaceFilterViewModel();
           // model.FinishHigh = new TimeSpan(10, 0, 0);
            
            return PartialView("~/Views/Shared/_FilterByDuration.cshtml",model);
        }
       

      

        //called from the racefilter
        [HttpPost]
        public ActionResult ViewResults(FilterViewModel queryModel)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(queryModel);
            return DisplayResultsView(filter);

        }

        //called from actions links in the Action Bar
         public ActionResult Display(SimpleFilterViewModel model)
        {
           var filter = new RaceFilterViewModel();
           filter.SaveRaceFilterValues(model);
           return DisplayResultsView(filter);

        }
               



        #region Protected Methods
        protected abstract ActionResult DisplayResultsView(RaceFilterViewModel model);

        protected virtual ActionResult DisplayResultsView(List<Triathlete> athletes, RaceFilterViewModel filter)
        {
            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = filter;
            viewmodel.Triathletes = athletes.ToPagedList(pageNumber: 1, pageSize: 100); //max xx per page
            viewmodel.TotalCount = athletes.Count();
            return View("List", viewmodel);
        }

        protected bool GetTrueOrFalseFromCheckbox(string val)
        {
            string[] values = val.Split(',');
            if (values != null && values.Length > 1)
                return true;
            else
                return false;

        }


        /// <summary>
        /// Get Stats for athletes associated with single race
        /// </summary>
        /// <param name="athletes"></param>
        /// <param name="race"></param>
        /// <returns></returns>
        protected TriStats GetStats(List<Triathlete> athletes, Race race)
        {
            TriStats stats = new TriStats(athletes, race);

            TriStatsCalculator calc = new TriStatsCalculator(athletes);

            //median
            stats.Swim.Median = calc.TimeSpanMedian("Swim");
            stats.Bike.Median = calc.TimeSpanMedian("Bike");
            stats.Run.Median = calc.TimeSpanMedian("Run");
            stats.Finish.Median = calc.TimeSpanMedian("Finish");


            stats.DivRank.Median = Math.Floor(calc.IntMedian("DivRank"));
            stats.GenderRank.Median = Math.Floor(calc.IntMedian("GenderRank"));
            stats.OverallRank.Median = Math.Floor(calc.IntMedian("OverallRank"));
            stats.Points.Median = Math.Floor(calc.IntMedian("Points"));

            //avg
            stats.Swim.Average = calc.TimeSpanAverage("Swim");
            stats.Bike.Average = calc.TimeSpanAverage("Bike");
            stats.Run.Average = calc.TimeSpanAverage("Run");
            stats.Finish.Average = calc.TimeSpanAverage("Finish");


            stats.DivRank.Average = Math.Floor(calc.IntAverage("DivRank"));
            stats.GenderRank.Average = Math.Floor(calc.IntAverage("GenderRank"));
            stats.OverallRank.Average = Math.Floor(calc.IntAverage("OverallRank"));
            stats.Points.Average = Math.Floor(calc.IntAverage("Points"));

            //min
            stats.Swim.Min = calc.TimeSpanMin("Swim");
            stats.Bike.Min = calc.TimeSpanMin("Bike");
            stats.Run.Min = calc.TimeSpanMin("Run");
            stats.Finish.Min = calc.TimeSpanMin("Finish");


            stats.DivRank.Min = calc.IntMin("DivRank");
            stats.GenderRank.Min = calc.IntMin("GenderRank");
            stats.OverallRank.Min = calc.IntMin("OverallRank");
            stats.Points.Min = calc.IntMin("Points");

            //max
            stats.Swim.Max = calc.TimeSpanMax("Swim");
            stats.Bike.Max = calc.TimeSpanMax("Bike");
            stats.Run.Max = calc.TimeSpanMax("Run");
            stats.Finish.Max = calc.TimeSpanMax("Finish");


            stats.DivRank.Max = calc.IntMax("DivRank");
            stats.GenderRank.Max = calc.IntMax("GenderRank");
            stats.OverallRank.Max = calc.IntMax("OverallRank");
            stats.Points.Max = calc.IntMax("Points");


            //standard deviation
            stats.Swim.StandDev = calc.TimeSpanStandardDeviation("Swim");
            stats.Bike.StandDev = calc.TimeSpanStandardDeviation("Bike");
            stats.Run.StandDev = calc.TimeSpanStandardDeviation("Run");
            stats.Finish.StandDev = calc.TimeSpanStandardDeviation("Finish");

            
            var swimSplit = TriStatsCalculator.Split(athletes, "Swim");
            stats.Swim.FastestHalf = TriStatsCalculator.Split(swimSplit.Item1, "Swim");
            stats.Swim.SlowestHalf = TriStatsCalculator.Split(swimSplit.Item2, "Swim");

            var bikeSplit = TriStatsCalculator.Split(athletes, "Bike");
            stats.Bike.FastestHalf = TriStatsCalculator.Split(bikeSplit.Item1, "Bike");
            stats.Bike.SlowestHalf = TriStatsCalculator.Split(bikeSplit.Item2, "Bike");

            var runSplit = TriStatsCalculator.Split(athletes, "Run");
            stats.Run.FastestHalf = TriStatsCalculator.Split(runSplit.Item1, "Run");
            stats.Run.SlowestHalf = TriStatsCalculator.Split(runSplit.Item2, "Run");

            var finishSplit = TriStatsCalculator.Split(athletes, "Finish");
            stats.Finish.FastestHalf = TriStatsCalculator.Split(finishSplit.Item1, "Finish");
            stats.Finish.SlowestHalf = TriStatsCalculator.Split(finishSplit.Item2, "Finish");



            stats.Swim.Data = athletes.OrderBy(a => a.Swim).Select(a => a.Swim.ToString("hh\\:mm\\:ss")).ToArray();
            stats.Bike.Data = athletes.OrderBy(a => a.Bike).Select(a => a.Bike.ToString("hh\\:mm\\:ss")).ToArray();
            stats.Run.Data = athletes.OrderBy(a => a.Run).Select(a => a.Run.ToString("hh\\:mm\\:ss")).ToArray();
            stats.Finish.Data = athletes.OrderBy(a => a.Finish).Select(a => a.Finish.ToString("hh\\:mm\\:ss")).ToArray();

            stats.DNFCount = calc.NumberDNFs();

            return stats;
        }


        /// <summary>
        /// GetStats for athletes, independent of a race
        /// </summary>
        /// <param name="athletes"></param>
        /// <returns></returns>
        protected TriStatsExtended GetStats(List<Triathlete> athletes)
        {
            var stats = new TriStatsExtended(athletes);
            stats.Athletes = athletes.ToList();

            var calc = new TriStatsCalculatorExtended(stats.Athletes);

            //median
            stats.Swim.Median = calc.TimeSpanMedian("Swim");
            stats.Bike.Median = calc.TimeSpanMedian("Bike");
            stats.Run.Median = calc.TimeSpanMedian("Run");
            stats.Finish.Median = calc.TimeSpanMedian("Finish");


            stats.DivRank.Median = Math.Floor(calc.IntMedian("DivRank"));
            stats.GenderRank.Median = Math.Floor(calc.IntMedian("GenderRank"));
            stats.OverallRank.Median = Math.Floor(calc.IntMedian("OverallRank"));
            stats.Points.Median = Math.Floor(calc.IntMedian("Points"));

            //avg
            stats.Swim.Average = calc.TimeSpanAverage("Swim");
            stats.Bike.Average = calc.TimeSpanAverage("Bike");
            stats.Run.Average = calc.TimeSpanAverage("Run");
            stats.Finish.Average = calc.TimeSpanAverage("Finish");


            stats.DivRank.Average = Math.Floor(calc.IntAverage("DivRank"));
            stats.GenderRank.Average = Math.Floor(calc.IntAverage("GenderRank"));
            stats.OverallRank.Average = Math.Floor(calc.IntAverage("OverallRank"));
            stats.Points.Average = Math.Floor(calc.IntAverage("Points"));

            //min
            stats.Swim.Min = calc.TimeSpanMin("Swim");
            stats.Bike.Min = calc.TimeSpanMin("Bike");
            stats.Run.Min = calc.TimeSpanMin("Run");
            stats.Finish.Min = calc.TimeSpanMin("Finish");


            stats.DivRank.Min = calc.IntMin("DivRank");
            stats.GenderRank.Min = calc.IntMin("GenderRank");
            stats.OverallRank.Min = calc.IntMin("OverallRank");
            stats.Points.Min = calc.IntMin("Points");

            //max
            stats.Swim.Max = calc.TimeSpanMax("Swim");
            stats.Bike.Max = calc.TimeSpanMax("Bike");
            stats.Run.Max = calc.TimeSpanMax("Run");
            stats.Finish.Max = calc.TimeSpanMax("Finish");


            stats.DivRank.Max = calc.IntMax("DivRank");
            stats.GenderRank.Max = calc.IntMax("GenderRank");
            stats.OverallRank.Max = calc.IntMax("OverallRank");
            stats.Points.Max = calc.IntMax("Points");


            //standard deviation
            stats.Swim.StandDev = calc.TimeSpanStandardDeviation("Swim");
            stats.Bike.StandDev = calc.TimeSpanStandardDeviation("Bike");
            stats.Run.StandDev = calc.TimeSpanStandardDeviation("Run");
            stats.Finish.StandDev = calc.TimeSpanStandardDeviation("Finish");


            var swimSplit = TriStatsCalculator.Split(athletes, "Swim");
            stats.Swim.FastestHalf = TriStatsCalculator.Split(swimSplit.Item1, "Swim");
            stats.Swim.SlowestHalf = TriStatsCalculator.Split(swimSplit.Item2, "Swim");

            var bikeSplit = TriStatsCalculator.Split(athletes, "Bike");
            stats.Bike.FastestHalf = TriStatsCalculator.Split(bikeSplit.Item1, "Bike");
            stats.Bike.SlowestHalf = TriStatsCalculator.Split(bikeSplit.Item2, "Bike");

            var runSplit = TriStatsCalculator.Split(athletes, "Run");
            stats.Run.FastestHalf = TriStatsCalculator.Split(runSplit.Item1, "Run");
            stats.Run.SlowestHalf = TriStatsCalculator.Split(runSplit.Item2, "Run");

            var finishSplit = TriStatsCalculator.Split(athletes, "Finish");
            stats.Finish.FastestHalf = TriStatsCalculator.Split(finishSplit.Item1, "Finish");
            stats.Finish.SlowestHalf = TriStatsCalculator.Split(finishSplit.Item2, "Finish");



            stats.Swim.Data = athletes.OrderBy(a => a.Swim).Select(a => a.Swim.ToString("hh\\:mm\\:ss")).ToArray();
            stats.Bike.Data = athletes.OrderBy(a => a.Bike).Select(a => a.Bike.ToString("hh\\:mm\\:ss")).ToArray();
            stats.Run.Data = athletes.OrderBy(a => a.Run).Select(a => a.Run.ToString("hh\\:mm\\:ss")).ToArray();
            stats.Finish.Data = athletes.OrderBy(a => a.Finish).Select(a => a.Finish.ToString("hh\\:mm\\:ss")).ToArray();

            stats.DNFCount = calc.NumberDNFs();

            return stats;
        }


        #endregion //Protected Methods

        #region Private Methods
        private Tuple<int[],int[],int[]> ConvertToInt(string races,string agegroups,string genders)
        {
            return Tuple.Create(
                 Array.ConvertAll(races.Split(','), int.Parse),
                 Array.ConvertAll(agegroups.Split(','), int.Parse),
                 Array.ConvertAll(genders.Split(','), int.Parse));


        }

        /// <summary>
        /// Get the request context Ids for this filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        private List<int> GetRequestIds(RaceFilterViewModel filter)
        {

            string[] raceIds = filter.SelectedRaceIds.ToArray();
            int[] ageGroupIds = filter.SelectedAgeGroupIds.ToArray();
            int[] genderIds = filter.SelectedGenderIds.ToArray();

            
            var query = _DBContext.RequestContext
                             .Where(r => raceIds.Contains(r.RaceId))
                             .Where(r => ageGroupIds.Contains(r.AgeGroupId))
                             .Where(r => genderIds.Contains(r.GenderId))
                             .Select(r => r.RequestContextId);


            return query.ToList();

        }



        #endregion//Private Methods

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _DBContext.Dispose();
            }
            base.Dispose(disposing);
        } 
    }
   
}
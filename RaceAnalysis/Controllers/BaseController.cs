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
       

        [HttpPost]
        public ActionResult SearchByAthleteName(FormCollection form, string races, string agegroups, string genders)
        {
            return SearchOpenFieldQuery("name", form["SearchString"], races, agegroups, genders);
        }


        [HttpPost]
        public ActionResult SearchByTimeThresholds(FormCollection form, string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel();
            viewmodel.SaveRaceFilterValues(races, agegroups, genders);

            var swimMinsLow = Convert.ToInt16(form["swim-lowtime-value"].ZeroIfEmpty());
            var swimLow = new TimeSpan(0, swimMinsLow, 0);

            var swimMinsHigh = Convert.ToInt16(form["swim-hightime-value"].MaxIfEmpty());
            var swimHigh = new TimeSpan(0, swimMinsHigh, 0);

            var bikeMinsLow = Convert.ToInt16(form["bike-lowtime-value"].ZeroIfEmpty());
            var bikeLow = new TimeSpan(0, bikeMinsLow, 0);

            var bikeMinsHigh = Convert.ToInt16(form["bike-hightime-value"].MaxIfEmpty());
            var bikeHigh = new TimeSpan(0, bikeMinsHigh, 0);
            
            var runMinsLow = Convert.ToInt16(form["run-lowtime-value"].ZeroIfEmpty());
            var runLow = new TimeSpan(0, runMinsLow, 0);

            var runMinsHigh = Convert.ToInt16(form["run-hightime-value"].MaxIfEmpty());
            var runHigh = new TimeSpan(0, runMinsHigh, 0);

            var finishMinsLow = Convert.ToInt16(form["finish-lowtime-value"].ZeroIfEmpty());
            var finishLow = new TimeSpan(0, finishMinsLow, 0);

            var finishMinsHigh = Convert.ToInt16(form["finish-hightime-value"].MaxIfEmpty());
            var finishHigh = new TimeSpan(0, finishMinsHigh, 0);


            ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);

            var requestIds = GetRequestIds(viewmodel);
            var athletes = search.SearchByDuration(swimLow, swimHigh, bikeLow, bikeHigh, runLow, runHigh, finishLow,finishHigh,requestIds);
            return DisplayResultsView(athletes,viewmodel);
        }
        private ActionResult SearchOpenFieldQuery(string field, string query, string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel();
            viewmodel.SaveRaceFilterValues(races, agegroups, genders);

            ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);

            var athletes = search.SearchFieldQuery(field, query);

            return DisplayResultsView(athletes, viewmodel);
        }

        //called from the racefilter
        [HttpPost]
        public ActionResult ApplyRaceFilter(FilterViewModel queryModel)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(queryModel);
            return DisplayResultsView(filter);

        }

        //called from actions links in the Action Bar
       public ActionResult SelectedRaces(SimpleFilterViewModel model)
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


            stats.DivRank.Median = calc.IntMedian("DivRank");
            stats.GenderRank.Median = calc.IntMedian("GenderRank");
            stats.OverallRank.Median = calc.IntMedian("OverallRank");
            stats.Points.Median = calc.IntMedian("Points");

            //avg
            stats.Swim.Average = calc.TimeSpanAverage("Swim");
            stats.Bike.Average = calc.TimeSpanAverage("Bike");
            stats.Run.Average = calc.TimeSpanAverage("Run");
            stats.Finish.Average = calc.TimeSpanAverage("Finish");


            stats.DivRank.Average = calc.IntAverage("DivRank");
            stats.GenderRank.Average = calc.IntAverage("GenderRank");
            stats.OverallRank.Average = calc.IntAverage("OverallRank");
            stats.Points.Average = calc.IntAverage("Points");

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
        protected TriStatsExtended GetStats(IEnumerable<Triathlete> athletes)
        {
            var stats = new TriStatsExtended(athletes);
            stats.Athletes = athletes.ToList();

            var calc = new TriStatsCalculatorExtended(stats.Athletes);

            //median
            stats.Swim.Median = calc.TimeSpanMedian("Swim");
            stats.Bike.Median = calc.TimeSpanMedian("Bike");
            stats.Run.Median = calc.TimeSpanMedian("Run");
            stats.Finish.Median = calc.TimeSpanMedian("Finish");


            stats.DivRank.Median = calc.IntMedian("DivRank");
            stats.GenderRank.Median = calc.IntMedian("GenderRank");
            stats.OverallRank.Median = calc.IntMedian("OverallRank");
            stats.Points.Median = calc.IntMedian("Points");

            //avg
            stats.Swim.Average = calc.TimeSpanAverage("Swim");
            stats.Bike.Average = calc.TimeSpanAverage("Bike");
            stats.Run.Average = calc.TimeSpanAverage("Run");
            stats.Finish.Average = calc.TimeSpanAverage("Finish");


            stats.DivRank.Average = calc.IntAverage("DivRank");
            stats.GenderRank.Average = calc.IntAverage("GenderRank");
            stats.OverallRank.Average = calc.IntAverage("OverallRank");
            stats.Points.Average = calc.IntAverage("Points");

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

            int[] raceIds = filter.SelectedRaceIds.ToArray();
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
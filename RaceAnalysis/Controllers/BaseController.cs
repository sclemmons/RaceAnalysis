using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using X.PagedList;
using RaceAnalysis.Service.Interfaces;
using System.Linq;
using System.IO;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Configuration;
using System.Diagnostics;
using RaceAnalysis.ServiceSupport;

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
        [HttpGet]
        public ActionResult Filter(FilterViewModel queryModel)
        {
       
            var filter = new RaceFilterViewModel(queryModel);
        
            return DisplayResultsView(filter);

        }

        //called from one controller to another
        public ActionResult ViewResults(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel(model);
            return DisplayResultsView(filter);

        }
        //called from actions links in the Action Bar using the GET verb
        public ActionResult Display(SimpleFilterViewModel model)
        {
            RaceFilterViewModel filter;
            //the following is a workaround for some flawed javascript that doesn't always pass the params we need
            if (String.IsNullOrEmpty(model.Races))
            {
                var parms = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                model.Races = parms["RaceId"];

            }
            filter = new RaceFilterViewModel(model);
            
            return DisplayResultsView(filter);

        }

        public JsonResult ExportReport(string name,string imageData)
        {
            string fileName = Path.Combine(Server.MapPath("~/ExportImage"), 
                                name + ".png");
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
            string host = Request.Url.Host.Equals("localhost") ? Request.Url.Authority : Request.Url.Host;
            
                        
            string fileUrl = string.Format("http://{0}/ExportImage/{1}.png", host, name);

            return new JsonResult { Data = fileUrl };
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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

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


            Trace.TraceInformation("Stats Calulation took: " + stopwatch.Elapsed);
            stopwatch.Stop();


            return stats;
        }

        protected TriStats GetRaceStats(string raceId)
        {
            var aggr = _DBContext.RacesAggregates.Include("Race").
                                Where(r => r.RaceId == raceId && r.Segment.Equals("all")).SingleOrDefault();

            TriStats stats = new TriStats();

            stats.Race = aggr.Race;

            //median
            stats.Swim.Median = aggr.SwimMedian;
            stats.Bike.Median = aggr.BikeMedian;
            stats.Run.Median = aggr.RunMedian;
            stats.Finish.Median = aggr.FinishMedian;


            //stats.DivRank.Median = a;
            //stats.GenderRank.Median = Math.Floor(calc.IntMedian("GenderRank"));
            //stats.OverallRank.Median = Math.Floor(calc.IntMedian("OverallRank"));
            //stats.Points.Median = Math.Floor(calc.IntMedian("Points"));

         
            //min
            stats.Swim.Min = aggr.SwimFastest;
            stats.Bike.Min = aggr.BikeFastest;
            stats.Run.Min = aggr.RunFastest;
            stats.Finish.Min = aggr.FinishFastest;



            //max
            stats.Swim.Max = aggr.SwimFastest;
            stats.Bike.Max = aggr.BikeFastest;
            stats.Run.Max = aggr.RunFastest;
            stats.Finish.Max = aggr.FinishFastest;

            

            //standard deviation
            stats.Swim.StandDev = aggr.SwimStdDev;
            stats.Bike.StandDev = aggr.BikeStdDev;
            stats.Run.StandDev = aggr.RunStdDev;
            stats.Finish.StandDev = aggr.FinishStdDev;


            stats.DNFCount = aggr.DNFCount;

            return stats;

        }

        protected TriStats GetRaceDivisionStats(string raceId,int agegroup, int gender )
        {
            var aggr = _DBContext.AgeGroupAggregates.Include("Race").
                                Where(r => r.RaceId == raceId
                                        && r.AgeGroupId == agegroup
                                            && r.GenderId == gender).SingleOrDefault();

            TriStats stats = new TriStats();

            stats.Race = aggr.Race;

            //median
            stats.Swim.Median = aggr.SwimMedian;
            stats.Bike.Median = aggr.BikeMedian;
            stats.Run.Median = aggr.RunMedian;
            stats.Finish.Median = aggr.FinishMedian;

            //min
            stats.Swim.Min = aggr.SwimFastest;
            stats.Bike.Min = aggr.BikeFastest;
            stats.Run.Min = aggr.RunFastest;
            stats.Finish.Min = aggr.FinishFastest;


            //max
            stats.Swim.Max = aggr.SwimFastest;
            stats.Bike.Max = aggr.BikeFastest;
            stats.Run.Max = aggr.RunFastest;
            stats.Finish.Max = aggr.FinishFastest;



            //standard deviation
            stats.Swim.StandDev = aggr.SwimStdDev;
            stats.Bike.StandDev = aggr.BikeStdDev;
            stats.Run.StandDev = aggr.RunStdDev;
            stats.Finish.StandDev = aggr.FinishStdDev;


            stats.DNFCount = aggr.DNFCount;

            return stats;

        }


        protected List<Triathlete> GetAllAthletesForRaces(RaceFilterViewModel filter)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var allAthletes = new List<Triathlete>();
            var allAgeGroupIds = AgeGroup.Expand(new int[] { 0 });
            var allGenderIds = Gender.Expand(new int[] { 0 });

            //assuming a cache contains athletes for each race, get all athletes for each race
            foreach (var raceId in filter.SelectedRaceIds)
            {
                var athletes = _RaceService.GetAthletes(
                        new BasicRaceCriteria
                        {
                            SelectedRaceIds = { raceId },
                            SelectedAgeGroupIds = allAgeGroupIds,
                            SelectedGenderIds = allGenderIds
                        }
                    );

                if (athletes.Count > 0)
                {
                    allAthletes.AddRange(athletes);
                }
            }
            Trace.TraceInformation("AgeGroupCompare GetAllRaceInfo took: " + stopwatch.Elapsed);
            stopwatch.Reset();

            return allAthletes;
        }

        //filter the list of athletes by agegroup, gender, and range
        protected List<Triathlete> GetFilteredAthletes(List<Triathlete> allAthletes, RaceFilterViewModel filter)
        {
            var selectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds);
            var selectedGenderIds = Gender.Expand(filter.SelectedGenderIds);

            var athletes = allAthletes.Where(
                    a => selectedAgeGroupIds.Contains(a.RequestContext.AgeGroupId) &&
                    selectedGenderIds.Contains(a.RequestContext.GenderId)).ToList();

            return new BasicFilterProvider(athletes, filter).GetAthletes();
        }
        //filter the list of athletes by race, agegroup, gender, and range
        protected List<Triathlete> GetFilteredAthletes(string raceId, List<Triathlete> allAthletes, RaceFilterViewModel filter)
        {
            var selectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds);
            var selectedGenderIds = Gender.Expand(filter.SelectedGenderIds);
         
            var athletes = allAthletes.Where(
                a => a.RequestContext.RaceId.Equals(raceId, StringComparison.CurrentCultureIgnoreCase) &&
                selectedAgeGroupIds.Contains(a.RequestContext.AgeGroupId) &&
                    selectedGenderIds.Contains(a.RequestContext.GenderId)).ToList();

            return new BasicFilterProvider(athletes, filter).GetAthletes();
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
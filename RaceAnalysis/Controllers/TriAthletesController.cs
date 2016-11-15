using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using X.PagedList;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;
using System.Linq;

namespace RaceAnalysis.Controllers
{
    public class TriathletesController : BaseController
    {

        public TriathletesController(IRaceService service) : base(service) { }

        public ActionResult Index()
        {
                    
            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }

       

        public ActionResult List()
        {
            var viewmodel = new TriathletesViewModel();
            return View(viewmodel);

        }
        public ActionResult Compare(SimpleFilterViewModel selections)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(selections);


            var viewmodel = new TriathletesCompareViewModel();
            viewmodel.Filter = filter;

            //order them from fastest to slowest so the view can know who was first,second, last, etc.
            viewmodel.Triathletes = _DBContext.Triathletes.OrderBy(t=>t.Finish)
                    .Where(t => filter.SelectedAthleteIds.Contains(t.TriathleteId));

            viewmodel.Stats = GetStats(viewmodel.Triathletes);  
            
            return View("~/Views/Triathletes/Compare.cshtml", viewmodel);
        }
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


        public ActionResult Search()
        {

            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);

        }
        public ActionResult ReIndex()
        {
            var search = new ElasticSearchFacade(_DBContext);
            search.ReIndex();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Called while paging through athletes. We need to return just the partial view of athletes
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //
        public PartialViewResult DisplayPagedAthletes(int page, SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            page = page > 0 ? page : 1;
            int pageSize = 20;

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = filter.SelectedGenderIds
                    },
                    filter);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page


            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return PartialView("~/Views/Shared/_OnePageOfAthletes.cshtml", viewmodel);
        }

        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            string searchType = form["SearchType"];
            string field = form["SearchField"];
            string queryValue = form["ValueField"];
            /****
            if (searchType == "FreeSearch")
            {
                return SearchOpenFieldQuery(field, queryValue);

            }
            else if (searchType == "CountPerCountry")
            {
                ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);
                var model = search.SearchCountPerCountry();
                return View("SearchResults", model);
            }
            else if (searchType == "ThresholdTime")
            {
             
            }
            *****/
            return View();
        }


        private ActionResult DisplayPagedResults(int page, RaceFilterViewModel filter)
        {

            page = page > 0 ? page : 1;
            int pageSize = 20;

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = filter.SelectedGenderIds
                    },
                    filter);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page


            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return PartialView("~/Views/Shared/_TriathletesCompare.cshtml", viewmodel);
        }


        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {

            int page =  1;
            int pageSize = 20;

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = filter.SelectedGenderIds
                    },
                    filter);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page
     
            
            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes =  onePageOfAthletes;
            viewmodel.Filter = filter;

            return  View("List", viewmodel);
        }

      



    }


}
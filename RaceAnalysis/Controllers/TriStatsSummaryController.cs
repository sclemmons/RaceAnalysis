using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Models;
using RaceAnalysis.Helpers;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.Controllers
{


    public class TriStatsSummaryController : BaseController
    {
        public TriStatsSummaryController(IRaceService races,ICacheService cache) : base(races,cache) { }


        // GET: Summary
        [Route("analyze")]
        public ActionResult Index()
        {
            var viewmodel = new HistogramViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }


      
        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            var viewModel = CreateHistogramViewModel(filter);
            return View("Histogram", viewModel);
        }

        private HistogramViewModel CreateHistogramViewModel(RaceFilterViewModel filter)
        {
            var viewModel = new HistogramViewModel();
            viewModel.Filter = filter;
            var athletes = GetFilteredAthletes(GetAllAthletesForRaces(filter), filter);

            var calculator = new TriStatsCalculator(athletes);

            viewModel.SwimMedian = calculator.TimeSpanMedian("Swim");
            viewModel.BikeMedian = calculator.TimeSpanMedian("Bike");
            viewModel.RunMedian = calculator.TimeSpanMedian("Run");
            viewModel.FinishMedian = calculator.TimeSpanMedian("Finish");

            viewModel.Triathletes = athletes;
            return viewModel;
        }
    }
}
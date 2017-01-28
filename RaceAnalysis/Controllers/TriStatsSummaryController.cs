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
        public TriStatsSummaryController(IRaceService service) : base(service) { }


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
            var athletes = new List<Triathlete>();

            foreach (string raceId in filter.SelectedRaceIds)
            {
                var athletesPerRace = _RaceService.GetAthletes(
                       new BasicRaceCriteria
                       {
                           SelectedRaceIds = new string[] { raceId },
                           SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                           SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                       },
                       filter
                    );

                athletes.AddRange(athletesPerRace); //add this group to our list
            }

            var calculator = new TriStatsCalculator(athletes);

            viewModel.SwimMedian = calculator.TimeSpanMedian("Swim");
            viewModel.BikeMedian = calculator.TimeSpanMedian("Bike");
            viewModel.RunMedian = calculator.TimeSpanMedian("Run");
            viewModel.FinishMedian = calculator.TimeSpanMedian("Finish");

            //  var test = calculator.TimeSpanHistogram("Finish");
            //  var test2 = calculator.TimeSpanHistogram("Bike");

            viewModel.Triathletes = athletes;
            return viewModel;
        }
    }
}
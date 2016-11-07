using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Models;
using RaceAnalysis.Helpers;

namespace RaceAnalysis.Controllers
{
    public class TriStatsHistogramController : BaseController
    {
        // GET: TriStatsHistogram
        public ActionResult Index()
        {
            var viewmodel = new HistogramViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }

        protected override ActionResult DisplayResultsView(int page, RaceFilterViewModel filter)
        {
            var viewModel = new HistogramViewModel();
            viewModel.Filter = filter;
        
            var athletes = new List<Triathlete>();

            foreach (int raceId in filter.SelectedRaceIds)
            {
                var athletesPerRace = _DAL.GetAthletes(new int[] { raceId }, filter.SelectedAgeGroupIds, filter.SelectedGenderIds);

                athletes.AddRange(athletesPerRace); //add this group to our list
            }

            var calculator = new TriStatsCalculator(athletes);
            viewModel.FinishMedian = calculator.TimeSpanMedian("Finish");


            viewModel.Triathletes = athletes;
            return View("Histogram", viewModel);
        }
    }
}
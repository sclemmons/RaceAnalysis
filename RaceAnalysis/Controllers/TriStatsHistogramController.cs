using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        protected override ActionResult DisplayResultsView(int page, int[] raceIds, int[] agegroupIds, int[] genderIds)
        {
            var viewModel = new HistogramViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.Filter.SaveRaceFilterValues(raceIds, agegroupIds, genderIds);

            var athletes = new List<Triathlete>();

            foreach (int raceId in raceIds)
            {
                var athletesPerRace = GetAthletes(new int[] { raceId }, agegroupIds, genderIds);
                                                                
                athletes.AddRange(athletesPerRace); //add this group to our list
            }

            var calculator = new TriStatsCalculator(athletes);
            viewModel.FinishMedian = calculator.TimeSpanMedian("Finish");


            viewModel.Triathletes = athletes;
            return View("Histogram", viewModel);
        }
    }
}
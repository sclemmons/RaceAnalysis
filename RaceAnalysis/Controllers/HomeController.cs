using RaceAnalysis.Service.Interfaces;
using System.Web.Mvc;
using RaceAnalysis.Models;
using System;


namespace RaceAnalysis.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IRaceService raceService, ICacheService cache) :base(raceService,cache)
        {

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {
            var viewModel = new HomePageViewModel();
            viewModel.RecentRaces = _RaceService.GetRacesMostRecent(10);
            viewModel.FastestIMSwims = _RaceService.GetRacesFastestSwim(10,"140.6");
            viewModel.FastestIMBikes = _RaceService.GetRacesFastestBike(10, "140.6");
            viewModel.FastestIMRuns = _RaceService.GetRacesFastestRun(10, "140.6");
            viewModel.FastestIMFinishes = _RaceService.GetRacesFastestFinish(10, "140.6");
            viewModel.FastestMaleFinishesIM = _RaceService.GetAthletesFastestFinish(10, "140.6","M");
            viewModel.FastestFemaleFinishesIM = _RaceService.GetAthletesFastestFinish(10, "140.6","F");

            return View(viewModel);
        }
        public ActionResult GetRace(string raceId)
        {
            if (String.IsNullOrEmpty(raceId))
                return new HttpNotFoundResult();

            var simpleModel = SimpleFilterViewModel.Create();
            simpleModel.Races = raceId;
            return RedirectToAction("ViewResults", "TriStatsSummary", simpleModel);

        }
        public ActionResult GetAthlete(string raceId)
        {
            if (String.IsNullOrEmpty(raceId))
                return new HttpNotFoundResult();

            var simpleModel = SimpleFilterViewModel.Create();
            simpleModel.Races = raceId;
            return RedirectToAction("ViewResults", "TriStatsSummary", simpleModel);

        }
        public ActionResult About()
        {
            ViewBag.Message = "TBD....";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Information...";

            return View();
        }

        protected override ActionResult DisplayResultsView(RaceFilterViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
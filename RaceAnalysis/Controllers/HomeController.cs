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
            viewModel.FastestSwims = _RaceService.GetRacesFastestSwim(10,"140.6");
            viewModel.FastestBikes = _RaceService.GetRacesFastestBike(10, "140.6");
            viewModel.FastestRuns = _RaceService.GetRacesFastestRun(10, "140.6");
            viewModel.FastestFinishes = _RaceService.GetRacesFastestFinish(10, "140.6");


            return View(viewModel);
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
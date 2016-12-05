using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysis.Controllers
{
    public class SearchController : BaseController
    {
        private IRaceService _raceService;
        public SearchController(IRaceService service) : base(service) { _raceService = service;}

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            return View();
        }

        public ActionResult Search(FormCollection form)
        {
         
            if(!String.IsNullOrEmpty(form["SearchByName"]))
            {
                return SearchByAthleteName(form["SearchByName"]);
            }
            else if(!String.IsNullOrEmpty(form["SearchRaceByConditions"]))
            {
               return SearchByRaceCondition(form["SearchByName"]);
            }

            return HttpNotFound();

        }

        public PartialViewResult SearchByRaceCondition(string searchstring)
        {
            var races =  _raceService.GetRacesByCondition( searchstring );

            return PartialView("SearchRaceResults");
        }

       
        public PartialViewResult SearchByAthleteName(string searchString)
        {
            var viewmodel = new TriathletesViewModel();

            List<Triathlete> athletes;
            if (!String.IsNullOrEmpty(searchString))
            {
                athletes = _raceService.GetAthletesByName(searchString);
       
            }
            else
            {
                athletes = new List<Triathlete>();
            }
            viewmodel.Triathletes = athletes;
            viewmodel.TotalCount = athletes.Count();

            return PartialView("~/Views/Shared/_OnePageOfAthletesNoFilter.cshtml", viewmodel);
        }

    }
}
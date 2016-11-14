using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    public class TriathletesCompareController : BaseController
    {

        public TriathletesCompareController(IRaceService service) : base(service) { }

        // GET: TriathletesCompare
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CompareAthletes(SimpleFilterViewModel selections)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(selections);


            var model = new TriathletesCompareViewModel();
            model.Triathletes = _DBContext.Triathletes
                    .Where(t => filter.SelectedAthleteIds.Contains(t.TriathleteId));

            return View("~/Views/TriathletesCompare/Compare.cshtml",model);
        }

        protected override ActionResult DisplayResultsView(RaceFilterViewModel model)
        {
            throw new NotImplementedException();

        }

    }
}
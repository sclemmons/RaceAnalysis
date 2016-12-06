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
        public SearchController(IRaceService service) : base(service) { }

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            return View();
        }

        


    }
}
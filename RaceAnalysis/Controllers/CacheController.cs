using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers

    [Authorize(Roles = "Admin")]
    public class CacheController : Controller
    {
        private ICacheService _Cache;
        private RaceAnalysisDbContext _DbContext; 

        public CacheController(ICacheService cacheService, RaceAnalysisDbContext db)
        {
            _Cache =cacheService;
            _DbContext = db;            
        }

        // GET: Cache
        public ActionResult Index()
        {
            var viewModel = new CacheViewModel();
            viewModel.ShallowAthleteCount = _Cache.GetShallowAthletes().Count;
            return View(viewModel);
        }

        public ActionResult FlushShallowAthletes()
        {
            var viewModel = new CacheViewModel();

            _Cache.FlushShallowAthletes();
            viewModel.ShallowAthleteCount = _Cache.GetShallowAthletes().Count;
            return View("Index",viewModel);
        }

        public ActionResult FillShallowAthletes()
        {
            
            var q = _DbContext.Triathletes.Include("RequestContext.Race");
            var r = q.OrderBy(t => t.Name).Select(t => new ShallowTriathlete()
            { Name = t.Name, Id = t.TriathleteId, RaceId = t.RequestContext.Race.RaceId });

            var athletes = r.ToList<ShallowTriathlete>();
            
            _Cache.PopulateCache(athletes);
            var viewModel = new CacheViewModel();
            viewModel.ShallowAthleteCount = _Cache.GetShallowAthletes().Count;
           return View("Index",viewModel);
        }
    }
}
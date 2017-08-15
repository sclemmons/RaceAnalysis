using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using X.PagedList;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;
using System.Linq;
using System;
using System.Web;
using Newtonsoft.Json;
using System.Diagnostics;

namespace RaceAnalysis.Controllers
{
    public class TriathletesController : BaseController
    {
        private ISearchService _SearchService;
        public TriathletesController(IRaceService raceService,ISearchService search,ICacheService cache) : base(raceService,cache)
        {
            _SearchService = search;
        }

        public ActionResult Index(string raceSort,string nameSort)
        {
            ViewBag.RaceSortParm = String.IsNullOrEmpty(raceSort) ? "race_desc" : "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(nameSort) ? "name_desc" : "";




            var filter = CurrentFilter;

            return DisplayResultsView(new RaceFilterViewModel(filter));
        }

        [HttpPost]
        public JsonResult GetAthletes()
        {
            var filter = new RaceFilterViewModel(CurrentFilter); //we may be able to pass this via the datatable options - check it out

            var search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            Trace.TraceInformation("GetAthletes-1" );

            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            if (String.IsNullOrEmpty(sortColumn))
                sortColumn = "Finish";
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                     
            string sort = null;
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                sort = sortColumn + " " + sortColumnDir;
            }

            List<Triathlete> athletes;

            Trace.TraceInformation("GetAthletes-sort&search:" + sort + "--" + search + " -- " );

            athletes = GetFilteredAthletes(GetAllAthletesForRaces(filter), filter, sort,search);
            Trace.TraceInformation("GetAthletes-2");

            recordsTotal = athletes.Count();
            Trace.TraceInformation("GetAthletes-2.1");

            var data = athletes.Skip(skip).Take(pageSize).ToList();
            Trace.TraceInformation("GetAthletes-3");

            var result = new JsonNetResult();
            result.Data = 
                   new
                   {
                       draw = draw,
                       recordsFiltered = recordsTotal,
                       recordsTotal = recordsTotal,
                       data = data
                   };

            Trace.TraceInformation("GetAthletes-4");

            return result;
        }

       
        public ActionResult List()
        {
            var viewmodel = new TriathletesViewModel();
            return View(viewmodel);

        }
        public ActionResult Compare(SimpleFilterViewModel model)
        {
            RaceFilterViewModel filter;

            //the following is a workaround for some flawed javascript that doesn't always pass the params we need
            if (String.IsNullOrEmpty(model.Races))
            {
                var simple = SimpleFilterViewModel.Create(Request.UrlReferrer.Query);
                if (!String.IsNullOrEmpty(model.selectedAthletes))
                {
                    simple.selectedAthletes = model.selectedAthletes;
                    return RedirectToAction("Compare", simple);
                }

                //this is to keep from a null exception
                filter = new RaceFilterViewModel(simple); 
                
            }
            else
            {
                filter = new RaceFilterViewModel(model);

            }
            var viewmodel = new TriathletesCompareViewModel();
            viewmodel.Filter = filter;

            //order them from fastest to slowest so the view can know who was first,second, last, etc.
            viewmodel.Triathletes = _DBContext.Triathletes.Include("RequestContext.Race").OrderBy(t=>t.Finish)
                    .Where(t => filter.SelectedAthleteIds.Contains(t.TriathleteId));

            viewmodel.Stats = GetStats(viewmodel.Triathletes.ToList());  
            
            return View("~/Views/Triathletes/Compare.cshtml", viewmodel);
        }

       
        //athletes/search?name=
        [Route("athletes/search")]
        public ActionResult Search(string selectedAthleteName)
        {
            string name;

            if (!String.IsNullOrEmpty(selectedAthleteName))
            {
                name = selectedAthleteName;
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["name"]))
            {
                name = Request.QueryString["name"];
            }
            else
            { 
                return View(new TriathletesViewModel());
            }
            
            
            return View(DoSearchAllByName(name));

           
        }
        //athletes/search?name=
        [Route("athletes/searchrace")]
        public ActionResult SearchRace(string raceId,string name)
        {
            if (String.IsNullOrEmpty(name))
                return View(new TriathletesViewModel());
            else
            {
                return View("List",DoSearchRaceByName(raceId,name));

            }
        }

       
        //search athletes by id
        public PartialViewResult AthleteSearch(string SelectedAthleteId)
        {

            if(SelectedAthleteId == "0")
            {
                var simple = SimpleFilterViewModel.Create(Request.UrlReferrer.Query);
                return DisplayPagedAthletes(1, simple);

            }

            return PartialView("_SearchResults-Race", DoSearchById(SelectedAthleteId));
        }
        
        //search all athletes we have by name and return details of all their races
        public PartialViewResult AthleteSearchAllByName(string athletes)
        {

            if (athletes == "0")
            {
                var simple = SimpleFilterViewModel.Create(Request.UrlReferrer.Query);
                return DisplayPagedAthletes(1, simple);

            }

            return PartialView("_SearchResults-All", DoSearchAllByName(athletes));
        }


        
        public ActionResult TypeaheadAthleteSearch(string query)
        {
            string[] raceIds = null;
            if (Request.UrlReferrer.Query != null)
            {
                var parms = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                string races = parms["races"];
                if (!String.IsNullOrEmpty(races))
                    raceIds = races.EmptyIfNull().Split(',');
            }
 
            var athletes = DoSearchX(query,raceIds);
            return Json(athletes, JsonRequestBehavior.AllowGet);
           
        }

        private List<ShallowTriathlete> DoSearchX(string search,string[] raceIds=null)
        {
           
            List<ShallowTriathlete> athletes;
            if (!String.IsNullOrEmpty(search))
            {
                 athletes = _SearchService.SearchAthletesByName(search,raceIds);
                
            }
            else
            {
                athletes = new List<ShallowTriathlete>();
            }
            return athletes;
        }

        private TriathletesViewModel DoSearchById(string id)
        { 
            var viewmodel = new TriathletesViewModel();
            var athletes = new List<Triathlete>();

            if (String.IsNullOrEmpty(id))
            {
                athletes = new List<Triathlete>();
                viewmodel.RaceStats = new TriStats();
                return viewmodel;
            }

            
            var a = _RaceService.GetAthleteById(Convert.ToInt32(id));
            athletes.Add(a);
            viewmodel.RaceStats = GetRaceStats(a.Race.RaceId);
            viewmodel.SelectedAthleteName = a.Name;
            viewmodel.Triathletes = athletes;
            viewmodel.TotalCount = athletes.Count();
            viewmodel.RaceDivisionStats = GetRaceDivisionStats(a.Race.RaceId, a.RequestContext.AgeGroupId, a.RequestContext.GenderId);
            viewmodel.SelectedAgeGroup = a.RequestContext.AgeGroup.DisplayName;
            viewmodel.SelectedGender = a.RequestContext.Gender.DisplayName;
               
            return viewmodel;
        }
        private TriathletesViewModel DoSearchRaceByName(string raceId,string name)
        {
            var viewmodel = new TriathletesViewModel();
            var athletes = new List<Triathlete>();
            if (String.IsNullOrEmpty(name))
            {
                athletes = new List<Triathlete>();
                viewmodel.RaceStats = new TriStats();
                return viewmodel;
            }

            //only one athlete supported
            var a = _RaceService.GetAthletesByName(name, new[] { raceId }).First();
            if(a == null)
            {
                return new TriathletesViewModel();
            }

            athletes.Add(a);
            viewmodel.RaceStats = GetRaceStats(a.Race.RaceId);
            viewmodel.SelectedAthleteName = a.Name;
            viewmodel.Triathletes = athletes;
            viewmodel.TotalCount = athletes.Count();
            viewmodel.RaceDivisionStats = GetRaceDivisionStats(a.Race.RaceId, a.RequestContext.AgeGroupId, a.RequestContext.GenderId);
            viewmodel.SelectedAgeGroup = a.RequestContext.AgeGroup.DisplayName;
            viewmodel.SelectedGender = a.RequestContext.Gender.DisplayName;

            return viewmodel;
        }
        private TriathletesViewModel DoSearchAllByName(string name)
        {
            var viewmodel = new TriathletesViewModel();

            var athletes = new List<Triathlete>();
            if (!String.IsNullOrEmpty(name))
            {
                athletes = _RaceService.GetAthletesByName(name);
                
            }
            else
            {
                athletes = new List<Triathlete>();
            }
            viewmodel.SelectedAthleteName = name;
            viewmodel.Triathletes = athletes;
            viewmodel.TotalCount = athletes.Count();

            return viewmodel;
        }


        /// <summary>
        /// Called while paging through athletes. We need to return just the partial view of athletes
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //
        public PartialViewResult DisplayPagedAthletes(int page, SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel(model);
            page = page > 0 ? page : 1;
            int pageSize = 20;

            var athletes = GetFilteredAthletes(GetAllAthletesForRaces(filter), filter);
            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page


            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return PartialView("~/Views/Shared/_OnePageOfAthletesImproved.cshtml", viewmodel);
        }

     
        private ActionResult DisplayPagedResults(int page, RaceFilterViewModel filter)
        {

            page = page > 0 ? page : 1;
            int pageSize = 20;

            var athletes = GetFilteredAthletes(GetAllAthletesForRaces(filter), filter);
            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page


            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return PartialView("_TriathletesList", viewmodel);
        }


        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {

            int page =  1;
            int pageSize = 20;

            var athletes = GetFilteredAthletes(GetAllAthletesForRaces(filter), filter);
            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page
     
            
            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes =  onePageOfAthletes;
            viewmodel.Filter = filter;

            return  View("List", viewmodel);
        }

        private IPagedList<Triathlete> GetOnePageAthletes(RaceFilterViewModel filter)
        {

            int page = 1;
            int pageSize = 20;

            var athletes = GetFilteredAthletes(GetAllAthletesForRaces(filter), filter);
            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page

            return onePageOfAthletes;
         
        }




    }


}
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

namespace RaceAnalysis.Controllers
{
    public class TriathletesController : BaseController
    {

        public TriathletesController(IRaceService service) : base(service) { }

        public ActionResult Index()
        {
                    
            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
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
        public ActionResult Search(string name)
        {
            if (String.IsNullOrEmpty(name))
                return View(new TriathletesViewModel());
            else
            {
                throw new NotImplementedException();
                //return View(DoSearch(name));

            }
        }



        //called within Search page via ajax
        public PartialViewResult AthleteSearch(string SelectedAthleteId)
        {

            if(SelectedAthleteId == "0")
            {
                var simple = SimpleFilterViewModel.Create(Request.UrlReferrer.Query);
                return DisplayPagedAthletes(1, simple);

            }

            return PartialView("_SearchResults", DoSearchById(SelectedAthleteId));
        }

      
        public ActionResult JsonAthleteSearch(string query,string races)
        {

            var raceIds = races.EmptyIfNull().Split(',');

            var athletes = DoSearchX(query,raceIds);
            return Json(athletes, JsonRequestBehavior.AllowGet);
           
        }

        private List<ShallowTriathlete> DoSearchX(string search,string[] raceIds=null)
        {
           
            List<ShallowTriathlete> athletes;
            if (!String.IsNullOrEmpty(search))
            {
                 athletes = _RaceService.GetAthletesByName(search,raceIds);
                
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
            if (!String.IsNullOrEmpty(id))
            {
                var a = _RaceService.GetAthleteById(Convert.ToInt32(id));
                athletes.Add(a);
            }
            else
            {
                athletes = new List<Triathlete>();
            }
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

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                    },
                    filter);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page


            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return PartialView("~/Views/Shared/_OnePageOfAthletes.cshtml", viewmodel);
        }

     
        private ActionResult DisplayPagedResults(int page, RaceFilterViewModel filter)
        {

            page = page > 0 ? page : 1;
            int pageSize = 20;

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                    },
                    filter);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page


            var viewmodel = new TriathletesViewModel();
            viewmodel.TotalCount = athletes.Count;
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return PartialView("~/Views/Shared/_TriathletesCompare.cshtml", viewmodel);
        }


        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {

            int page =  1;
            int pageSize = 20;

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                    },
                    filter);

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

            List<Triathlete> athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                    },
                    filter);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page

            return onePageOfAthletes;
           // var viewmodel = new TriathletesViewModel();
           // viewmodel.TotalCount = athletes.Count;
           // viewmodel.Triathletes = onePageOfAthletes;
           // viewmodel.Filter = filter;

          //  return View("List", viewmodel);
        }




    }


}
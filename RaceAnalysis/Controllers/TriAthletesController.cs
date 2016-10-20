using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using X.PagedList;
using System.Linq.Dynamic;



namespace RaceAnalysis.Controllers
{
    public class TriathletesController : BaseController
    {

        public ActionResult Index()
        {
                    
            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = new RaceFilterViewModel(_DBContext);
            return View(viewmodel);
        }

       

        public ActionResult List()
        {
            var viewmodel = new TriathletesViewModel();
            return View(viewmodel);

        }
        public ActionResult Search()
        {

            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);

        }
        public ActionResult ReIndex()
        {
            var search = new ElasticSearchFacade(_DBContext);
            search.ReIndex();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Search(FormCollection form)
        {
            string searchType = form["SearchType"];
            string field = form["SearchField"];
            string queryValue = form["ValueField"];
            /****
            if (searchType == "FreeSearch")
            {
                return SearchOpenFieldQuery(field, queryValue);

            }
            else if (searchType == "CountPerCountry")
            {
                ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);
                var model = search.SearchCountPerCountry();
                return View("SearchResults", model);
            }
            else if (searchType == "ThresholdTime")
            {
             
            }
            *****/
            return View();
        }
     
              
      

        protected override ActionResult DisplayResultsView(int page, int[] races, int[] agegroups,int[] genders)
        {

            page = page > 0 ? page : 1;
            int pageSize = 20;

            //Since this action can be called directly from a different view than the race filter, 
            //specifically from the paging control
            //we've got to pass the values in and repopulate our filter
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(races, agegroups, genders);



            List<Triathlete> athletes = GetAthletes(races, agegroups, genders);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page
     
            
            var viewmodel = new TriathletesViewModel();
            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = filter;

            return View("List", viewmodel);
        }





    }


}
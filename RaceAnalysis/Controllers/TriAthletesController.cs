using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using X.PagedList;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysis.Controllers
{
    public class TriathletesController : BaseController
    {

        public TriathletesController(IRaceService service) : base(service) { }

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




        protected override ActionResult DisplayResultsView(int page, RaceFilterViewModel criteria)
        {

            page = page > 0 ? page : 1;
            int pageSize = 20;

            List<Triathlete> athletes = _RaceService.GetAthletes(criteria,criteria);

            var onePageOfAthletes = athletes.ToPagedList(page, pageSize); //max xx per page
     
            
            var viewmodel = new TriathletesViewModel();
            viewmodel.Triathletes =  onePageOfAthletes;
            viewmodel.Filter = criteria;

            return View("List", viewmodel);
        }

      






    }


}
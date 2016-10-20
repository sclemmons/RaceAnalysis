using Elasticsearch.Net;
using Nest;
using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace RaceAnalysis.Controllers
{
    public class SearchController : BaseController
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        protected override ActionResult DisplayResultsView(int page, int[] races, int[] agegroups, int[] genders)
        {
            return View();
        }
        public ActionResult SearchXXX()
        {
            var nodes = new Uri[]
           {
                new Uri("http://localhost:9200")
           };
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);
            var request = new SearchRequest
            {
                From = 0,
                Size = 100,
                Query = new MatchQuery { Field = "name", Query = "clemmons" }
            };

            var response = client.Search<Triathlete>(request);

            var viewmodel = new TriathletesViewModel();
            var athletes = response.Documents.ToList();
            var onePageOfAthletes = athletes.ToPagedList(pageNumber: 1, pageSize: 100); //max xx per page

            viewmodel.Triathletes = onePageOfAthletes;
            viewmodel.Filter = new RaceFilterViewModel();
           
            return View("List", viewmodel);

        }

    }
}
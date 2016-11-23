using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.Controllers
{
    public class TriStatsController : BaseController
    {
        public TriStatsController(IRaceService service) : base(service) { }


        
        // GET: TriStatsControllers
        public ActionResult Index()
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = new RaceFilterViewModel(); 
            return View(viewModel);
        }
       
        public ActionResult List()
        {

            var viewmodel = new TriathletesViewModel(); //switch to the Triathletes View
            viewmodel.Filter = new RaceFilterViewModel(); 
            return View(viewmodel);

        }

      /// <summary>
      /// DisplayReview() - In this version the group of athletes are given to us. We just need to calulate the stats and return the view
      /// </summary>
      /// <param name="athletes"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
        protected override ActionResult DisplayResultsView(List<Triathlete> athletes,RaceFilterViewModel filter)
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = filter;
            
            foreach (var raceId in filter.SelectedRaceIds)   
            {
                var subset = athletes.Where(a => a.Race.RaceId == raceId).ToList();
                var stats = GetStats(subset,filter.
                                AvailableRaces.Single(r=>r.RaceId ==raceId)); //calculate the stats based on each race 
                viewModel.Stats.Add(stats);
            }
            //TO-DO: Look at this for sharing views:
            //https://blog.michaelckennedy.net/2012/03/06/managing-shared-views-folder-for-large-mvc-projects/
            //
            return View("~/Views/TriStats/TriStats.cshtml", viewModel);
        }
     
        /// <summary>
        /// DisplayResultsView() - In this version we must iterate through each race, get the athletes, and calculate their stats
        /// </summary>
        /// <param name="page"></param>
        /// <param name="raceIds"></param>
        /// <param name="agegroupIds"></param>
        /// <param name="genderIds"></param>
        /// <returns></returns>
        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = filter;
           

            foreach (int raceId in filter.SelectedRaceIds) //it makes more sense to split the races in order to compare them rather than to combine their stats
            {
             
                var athletes = _RaceService.GetAthletes(
                      new BasicRaceCriteria
                      {
                          SelectedRaceIds = new int[] { raceId },
                          SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                          SelectedGenderIds = filter.SelectedGenderIds
                      }, 
                      filter
                );
                var stats = GetStats(athletes, filter.AvailableRaces.Single(r => r.RaceId == raceId));
                viewModel.Stats.Add(stats);
            }

            return View("~/Views/TriStats/TriStats.cshtml", viewModel);
        }
      
    }
}
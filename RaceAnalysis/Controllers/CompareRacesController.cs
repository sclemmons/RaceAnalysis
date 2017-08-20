using RaceAnalysis.Models;
using RaceAnalysis.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.Controllers
{
    public class CompareRacesController : TriStatsController
    {
        public CompareRacesController(IRaceService races,ICacheService cache) : base(races,cache) { }


        public ActionResult Compare()
        {
            var viewmodel = new TriStatsViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }
        
      
        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = filter;

            List<Triathlete> allAthletes = GetAllAthletesForRaces(filter);


            var selectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds);
            var selectedGenderIds = Gender.Expand(filter.SelectedGenderIds);
            var athletes = new List<Triathlete>();
       
            foreach (string raceId in filter.SelectedRaceIds) 
            {
                var athletesPerRace = allAthletes.Where(
                    a => a.RequestContext.RaceId.Equals(raceId,System.StringComparison.CurrentCultureIgnoreCase) &&
                    selectedAgeGroupIds.Contains(a.RequestContext.AgeGroupId) &&
                    selectedGenderIds.Contains(a.RequestContext.GenderId)
                    ).ToList();
                    // new BasicDurationFilter() { } //bypass the user's duration filter so we can get all athletes, including DNFs
                
                
                athletes.AddRange(athletesPerRace);
                viewModel.Stats.Add(
                    GetStats(athletesPerRace, filter.AvailableRaces.Single(r => r.RaceId == raceId)));
            }
            viewModel.Triathletes = athletes;
            return View("Compare", viewModel);
        }
    }
}
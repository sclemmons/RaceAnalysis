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
        public CompareRacesController(IRaceService service) : base(service) { }


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
        
            var athletes = new List<Triathlete>();
       
            foreach (string raceId in filter.SelectedRaceIds) 
            {
                var athletesPerRace = _RaceService.GetAthletes(
                     new BasicRaceCriteria
                     {
                         SelectedRaceIds = new string[] { raceId },
                         SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                         SelectedGenderIds = Gender.Expand( filter.SelectedGenderIds)
                     },
                     new BasicDurationFilter() { } //bypass the user's duration filter so we can get all athletes, including DNFs
                 );
                athletes.AddRange(athletesPerRace);
                viewModel.Stats.Add(GetStats(athletesPerRace, filter.AvailableRaces.Single(r => r.RaceId == raceId)));
            }
            viewModel.Triathletes = athletes;
            return View("Compare", viewModel);
        }
    }
}
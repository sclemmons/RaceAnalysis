using RaceAnalysis.Models;
using RaceAnalysis.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.Controllers
{
    public class TriStatsRaceComparerController : TriStatsController
    {
        public TriStatsRaceComparerController(IRaceService service) : base(service) { }


        public ActionResult Compare()
        {
            var viewmodel = new TriStatsViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }

        protected override ActionResult DisplayResultsView(int page, RaceFilterViewModel filter)
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = filter;
        
            var athletes = new List<Triathlete>();
       
            foreach (int raceId in filter.SelectedRaceIds) 
            {
                var athletesPerRace = _RaceService.GetAthletes(
                     new BasicRaceCriteria
                     {
                         SelectedRaceIds = new int[] { raceId },
                         SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                         SelectedGenderIds = filter.SelectedGenderIds
                     }, 
                     filter
                 );
                athletes.AddRange(athletesPerRace);
                viewModel.Stats.Add(GetStats(athletesPerRace, filter.AvailableRaces.Single(r => r.RaceId == raceId)));
            }
       
            return View("Compare", viewModel);
        }
    }
}
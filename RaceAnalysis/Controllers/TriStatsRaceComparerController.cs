using RaceAnalysis.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    public class TriStatsRaceComparerController : TriStatsController
    {
        // GET: TriStatsRaceComparer

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
                var athletesPerRace = _DAL.GetAthletes(new int[] { raceId }, filter.SelectedAgeGroupIds, filter.SelectedGenderIds);
                athletes.AddRange(athletesPerRace);
                viewModel.Stats.Add(GetStats(athletesPerRace, _DBContext.Races.Single(r => r.RaceId == raceId)));
            }
       
            return View("Compare", viewModel);
        }
    }
}
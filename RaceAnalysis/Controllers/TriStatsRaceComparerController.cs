using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        protected override ActionResult DisplayResultsView(int page, int[] raceIds, int[] agegroupIds, int[] genderIds)
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.Filter.SaveRaceFilterValues(raceIds, agegroupIds, genderIds);

            var athletes = new List<Triathlete>();
       
            foreach (int raceId in raceIds) 
            {
                var athletesPerRace = _DAL.GetAthletes(new int[] { raceId }, agegroupIds, genderIds);
                athletes.AddRange(athletesPerRace);
                viewModel.Stats.Add(GetStats(athletesPerRace, _DBContext.Races.Single(r => r.RaceId == raceId)));
            }
       
            return View("Compare", viewModel);
        }
    }
}
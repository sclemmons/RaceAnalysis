using RaceAnalysis.Models;
using RaceAnalysis.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;
using System.Diagnostics;

namespace RaceAnalysis.Controllers
{
    public class TriStatsAgeGroupsCompareController : TriStatsController
    {
        public TriStatsAgeGroupsCompareController(IRaceService service) : base(service) { }

        public ActionResult Compare()
        {
            var viewmodel = new AgeGroupCompareViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }

        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            var viewModel = new AgeGroupCompareViewModel();
            viewModel.Filter = filter;

            List<Triathlete> allAthletes= GetAllAthletesForRaces(filter);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var selectedGenderIds = Gender.Expand(filter.SelectedGenderIds);

            var resultingAthletes = new List<Triathlete>();
            //calculating each selected age groups so that we can do the same when we draw the chart
            foreach (var agId in AgeGroup.Expand(viewModel.Filter.SelectedAgeGroupIds)) //collect the stats for each age group
            {
                var athletesPerAG = allAthletes.Where(
                            a => a.RequestContext.AgeGroupId == agId &&
                            selectedGenderIds.Contains(a.RequestContext.GenderId)).ToList();

                var filteredAthletes = new BasicFilterProvider(athletesPerAG, filter).GetAthletes();

                resultingAthletes.AddRange(filteredAthletes);

                var stats = GetStats(filteredAthletes);
                stats.AgeGroupId = agId;
                viewModel.Stats.Add(stats);

            }

            Trace.TraceInformation("AgeGroupCompare Calulating all took: " + stopwatch.Elapsed);
            stopwatch.Stop();

            viewModel.Triathletes = resultingAthletes;
            return View("Compare", viewModel);
        }
 
    }
}
using RaceAnalysis.Models;
using RaceAnalysis.Service;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;


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

        protected override ActionResult DisplayResultsView(int page, RaceFilterViewModel filter)
        {
            var viewModel = new AgeGroupCompareViewModel();
            viewModel.Filter = filter;
            
            var athletes = new List<Triathlete>();

            //for now let's only deal with a single race to keep it simple
            int raceId = viewModel.Filter.SelectedRaceIds.First();


            //pulling from selected age groups so that we can do the same when we draw the chart
            foreach (var agId in viewModel.Filter.SelectedAgeGroupIds) //collect the stats for each age group
            {
                var athletesPerAG = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = AgeGroup.Expand(new int[] { agId }),
                        SelectedGenderIds = filter.SelectedGenderIds
                    },
                    filter
                  );
                athletes.AddRange(athletesPerAG);
                viewModel.Stats.Add(GetStats(athletesPerAG, viewModel.Filter.AvailableRaces.Single(r => r.RaceId == raceId)));
            }

            return View("Compare", viewModel);
        }

       
    }
}
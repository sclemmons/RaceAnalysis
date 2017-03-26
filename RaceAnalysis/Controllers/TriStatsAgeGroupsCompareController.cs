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

        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            var viewModel = new AgeGroupCompareViewModel();
            viewModel.Filter = filter;
            
            var athletes = new List<Triathlete>();

          //calculating each selected age groups so that we can do the same when we draw the chart
            foreach (var agId in AgeGroup.Expand(viewModel.Filter.SelectedAgeGroupIds)) //collect the stats for each age group
            {
                var athletesPerAG = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = filter.SelectedRaceIds,
                        SelectedAgeGroupIds = new int[] { agId },
                        SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                    },
                    filter
                  );
                athletes.AddRange(athletesPerAG);
                viewModel.Stats.Add(GetStats(athletesPerAG));
            }

            viewModel.Triathletes = athletes;
            return View("Compare", viewModel);
        }

       
    }
}
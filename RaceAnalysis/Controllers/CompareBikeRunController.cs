using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{

    public class CompareBikeRunController : TriStatsController
    {
        public CompareBikeRunController(IRaceService service) : base(service) { }

        //called from actions links in the Action Bar
        public ActionResult DisplayPacing(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            return DisplayResultsView(filter,"Pacing");

        }

        //called from actions links in the Action Bar
        public ActionResult DisplayQuartiles(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            return DisplayResultsView(filter,"Quartiles");

        }

        private ActionResult DisplayResultsView(RaceFilterViewModel filter, string viewName)
        {
            var viewModel = new CompareBikeRunViewModel();
            viewModel.Filter = filter;

            var allAthletes = new List<Triathlete>();
            foreach (string raceId in filter.SelectedRaceIds)
            {

                var athletes = _RaceService.GetAthletes(
                      new BasicRaceCriteria
                      {
                          SelectedRaceIds = new string[] { raceId },
                          SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                          SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                      },
                      filter
                );
                allAthletes.AddRange(athletes);
               
            }

            var calc = new TriStatsCalculator(allAthletes);
            
            viewModel.BikeMedian = calc.TimeSpanMedian("Bike");
            viewModel.RunMedian = calc.TimeSpanMedian("Run");
            
            viewModel.Triathletes = allAthletes;
            
            PartitionAthletes(viewModel);

            viewModel.Stats.Add(GetStats(allAthletes));


            return View(viewName, viewModel);
        }

        private void PartitionAthletes(CompareBikeRunViewModel viewModel)
        {
            foreach(var athlete in viewModel.Triathletes)
            {
                if(athlete.Bike <= viewModel.BikeMedian)
                {
                    if(athlete.Run <= viewModel.RunMedian)
                    {
                        viewModel.FastBikeFastRun.Add(athlete);
                    }
                    else
                    {
                        viewModel.FastBikeSlowRun.Add(athlete);
                    }

                }
                else
                {
                    if(athlete.Run <= viewModel.RunMedian)
                    {
                        viewModel.SlowBikeFastRun.Add(athlete);
                    }
                    else
                    {
                        viewModel.SlowBikeSlowRun.Add(athlete);
                    }

                }
            }

        }
    }

    
}
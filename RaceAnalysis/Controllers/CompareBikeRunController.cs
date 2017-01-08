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




        protected override ActionResult DisplayResultsView(RaceFilterViewModel filter)
        {
            var viewModel = new CompareBikeRunViewModel();
            viewModel.Filter = filter;

            var allAthletes = new List<Triathlete>();
            foreach (int raceId in filter.SelectedRaceIds)
            {

                var athletes = _RaceService.GetAthletes(
                      new BasicRaceCriteria
                      {
                          SelectedRaceIds = new int[] { raceId },
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


            return View("Compare", viewModel);
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
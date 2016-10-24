using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using System.Web.Helpers;
using System.Collections;


namespace RaceAnalysis.Controllers
{
    public class TriStatsController : BaseController
    {
      
        // GET: TriStatsControllers
        public ActionResult Index()
        {
            var viewmodel = new TriStatsViewModel();
            viewmodel.Filter = new RaceFilterViewModel(_DBContext); 
            return View(viewmodel);
        }
       
        public ActionResult List()
        {

            var viewmodel = new TriathletesViewModel(); //switch to the Triathletes View
            viewmodel.Filter = new RaceFilterViewModel(); 
            return View(viewmodel);

        }

      /// <summary>
      /// DisplayReview() - In this version the group of athletes are given to us. We just need to calulate the stats and return the view
      /// </summary>
      /// <param name="athletes"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
        protected override ActionResult DisplayResultsView(List<Triathlete> athletes,RaceFilterViewModel filter)
        {
            var viewmodel = new TriStatsViewModel();
            viewmodel.Filter = filter;

            foreach (var race in filter.SelectedRaces)   
            {
                var subset = athletes.Where(a => a.Race.RaceId == race.RaceId).ToList();
                var stats = GetStats(subset,race); //calculate the stats based on each race 
                viewmodel.Stats.Add(stats);
            }


            return View("TriStats",viewmodel);
        }
     
        /// <summary>
        /// DisplayResultsView() - In this version we must iterate through each race, get the athletes, and calculate their stats
        /// </summary>
        /// <param name="page"></param>
        /// <param name="raceIds"></param>
        /// <param name="agegroupIds"></param>
        /// <param name="genderIds"></param>
        /// <returns></returns>
        protected override ActionResult DisplayResultsView(int page, int[] raceIds, int[] agegroupIds, int[] genderIds)
        {
            var viewModel = new TriStatsViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.Filter.SaveRaceFilterValues(raceIds, agegroupIds, genderIds);
           

            foreach (int raceId in raceIds) //it makes more sense to split the races in order to compare them rather than to combine their stats
            {
             
                var athletes = GetAthletes(new int[]{ raceId }, agegroupIds, genderIds);
                var stats = GetStats(athletes, _DBContext.Races.Single(r => r.RaceId == raceId));
                viewModel.Stats.Add(stats);
            }

            return View("TriStats", viewModel);                       
        }
    


      
        protected TriStats GetStats(List<Triathlete> athletes, Race race)
        {
            TriStats stats = new TriStats(athletes, race);

            TriStatsCalculator calc = new TriStatsCalculator(athletes);

            //median
            stats.Swim.Median = calc.TimeSpanMedian("Swim");
            stats.Bike.Median = calc.TimeSpanMedian("Bike");
            stats.Run.Median = calc.TimeSpanMedian("Run");
            stats.Finish.Median = calc.TimeSpanMedian("Finish");


            stats.DivRank.Median = calc.IntMedian("DivRank");
            stats.GenderRank.Median = calc.IntMedian("GenderRank");
            stats.OverallRank.Median = calc.IntMedian("OverallRank");
            stats.Points.Median = calc.IntMedian("Points");

            //avg
            stats.Swim.Average = calc.TimeSpanAverage("Swim");
            stats.Bike.Average = calc.TimeSpanAverage("Bike");
            stats.Run.Average = calc.TimeSpanAverage("Run");
            stats.Finish.Average = calc.TimeSpanAverage("Finish");


            stats.DivRank.Average = calc.IntAverage("DivRank");
            stats.GenderRank.Average = calc.IntAverage("GenderRank");
            stats.OverallRank.Average = calc.IntAverage("OverallRank");
            stats.Points.Average = calc.IntAverage("Points");

            //min
            stats.Swim.Min = calc.TimeSpanMin("Swim");
            stats.Bike.Min = calc.TimeSpanMin("Bike");
            stats.Run.Min = calc.TimeSpanMin("Run");
            stats.Finish.Min = calc.TimeSpanMin("Finish");


            stats.DivRank.Min = calc.IntMin("DivRank");
            stats.GenderRank.Min = calc.IntMin("GenderRank");
            stats.OverallRank.Min = calc.IntMin("OverallRank");
            stats.Points.Min = calc.IntMin("Points");

            //max
            stats.Swim.Max = calc.TimeSpanMax("Swim");
            stats.Bike.Max = calc.TimeSpanMax("Bike");
            stats.Run.Max = calc.TimeSpanMax("Run");
            stats.Finish.Max = calc.TimeSpanMax("Finish");


            stats.DivRank.Max = calc.IntMax("DivRank");
            stats.GenderRank.Max = calc.IntMax("GenderRank");
            stats.OverallRank.Max = calc.IntMax("OverallRank");
            stats.Points.Max = calc.IntMax("Points");

            return stats;
        }
    }
}
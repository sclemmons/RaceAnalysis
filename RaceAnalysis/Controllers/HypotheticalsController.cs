using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.Controllers 
{
    public class HypotheticalsController : BaseController
    {

        public HypotheticalsController(IRaceService service) : base(service) { }

        // GET: Hypotheticals
      
        public ActionResult Index()
        {
            var viewModel = new HypotheticalsViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            //for this view, leave out the default selections
            viewModel.Filter.SelectedAgeGroupIds = new List<int>();
            viewModel.Filter.SelectedGenderIds = new List<int>();


            return View(viewModel);
        }
      
        public PartialViewResult FinishTime(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView= GetEstimatedTime(filter);
            return PartialView("_EstFinish", modelView);
        }
        public PartialViewResult SwimTime(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            return PartialView("_EstSwim", modelView);
        }
        public PartialViewResult BikeTime(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            return PartialView("_EstBike", modelView);
        }
        public PartialViewResult RunTime(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            return PartialView("_EstRun", modelView);
        }

        public PartialViewResult ShowBikeRange()
        {
            var modelView = new RaceFilterViewModel();
           
            return PartialView("_BikeRange", modelView);
        }
        /// <summary>
        /// Given the bike range, provide estimas 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult GivenBikeTime(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            return PartialView("_GivenBikeRange", modelView);
        }

        #region Protected Methods
        protected override ActionResult DisplayResultsView(RaceFilterViewModel model)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Private Methods
        private HypotheticalsViewModel GetEstimatedTime(RaceFilterViewModel filter)
        {
            var modelView = new HypotheticalsViewModel();
            modelView.Filter = filter;
             
            int raceId = filter.SelectedRaceIds.First();

            var athletes = _RaceService.GetAthletes(
                    new BasicRaceCriteria
                    {
                        SelectedRaceIds = new int[] { raceId },
                        SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                        SelectedGenderIds = Gender.Expand(filter.SelectedGenderIds)
                    },
                    filter
            );
            modelView.Stats.Add(GetStats(athletes, filter.AvailableRaces.Single(r => r.RaceId == raceId)));

            modelView.SelectedSkillLevel = filter.SkillLevel; //TO-DO: fix this up so it removes the redundancy

            return modelView;
        }

        #endregion

    }
}
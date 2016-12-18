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
      
        public PartialViewResult DisplayFinishTime(SimpleFilterViewModel model)
        {
            model.ClearDuration(); //we do this just in case they have values from the duration filters on the page
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView= GetEstimatedTime(filter);
            modelView.Filter = filter;
            return PartialView("_EstFinish", modelView);
        }
        public PartialViewResult DisplaySwimTime(SimpleFilterViewModel model)
        {
            model.ClearDuration(); //we do this just in case they have values from the duration filters on the page
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            modelView.Filter = filter;
            return PartialView("_EstSwim", modelView);
        }
        public PartialViewResult DisplayBikeTime(SimpleFilterViewModel model)
        {
            model.ClearDuration(); //we do this just in case they have values from the duration filters on the page
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            modelView.Filter = filter;

            return PartialView("_EstBike", modelView);
        }
        public PartialViewResult DisplayRunTime(SimpleFilterViewModel model)
        {
            model.ClearDuration(); //we do this just in case they have values from the duration filters on the page
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var modelView = GetEstimatedTime(filter);
            modelView.Filter = filter;

            return PartialView("_EstRun", modelView);
        }

        public PartialViewResult ShowBikeRangeForFinish()
        {
            var viewModel = new HypotheticalsViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.SelectedSplit = "BikeFinish";

            return PartialView("_BikeRange", viewModel);
        }
        public PartialViewResult ShowBikeRangeForRun()
        {
            var viewModel = new HypotheticalsViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.SelectedSplit = "BikeRun";

            return PartialView("_BikeRange", viewModel);
        }
        public PartialViewResult ShowRunRangeForFinish()
        {
            var viewModel = new HypotheticalsViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.SelectedSplit = "RunFinish";
            return PartialView("_RunRange", viewModel);
        }
        /// <summary>
        /// Given the bike range, provide estimates 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult DisplayBikeFinish(SimpleFilterViewModel model)
        {
            model.runhightimevalue = null; //clear the values that we want to ignore in this hypothetical (everything but bike)
            model.runlowtimevalue = null;
            model.swimhightimevalue = null;
            model.swimlowtimevalue = null;

            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);

            var modelView = GetEstimatedTime(filter);
            modelView.SelectedSplit = "BikeFinish"; //this value gets used by the generic _finishHistogram
            modelView.Filter = filter;

            return PartialView("_GivenBikeRangeFinish", modelView);
        }
        /// <summary>
        /// Given the bike range, provide estimates for run
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult DisplayBikeRun(SimpleFilterViewModel model)
        {
            model.runhightimevalue = null; //clear the values that we want to ignore in this hypothetical (everything but bike)
            model.runlowtimevalue = null;
            model.swimhightimevalue = null;
            model.swimlowtimevalue = null;

            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);

            var modelView = GetEstimatedTime(filter);
            modelView.SelectedSplit = "BikeRun"; //this value gets used by the generic _finishHistogram
            modelView.Filter = filter;

            return PartialView("_GivenBikeRangeRun", modelView);
        }
        // <summary>
        /// Given the run range, provide estimates. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult DisplayRunFinish(SimpleFilterViewModel model)
        {
            model.bikehightimevalue = null; //clear the values that we want to ignore in this hypothetical (everything but run)
            model.bikelowtimevalue = null;
            model.swimhightimevalue = null;
            model.swimlowtimevalue = null;

            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);

            var modelView = GetEstimatedTime(filter);
            modelView.Filter = filter;
            modelView.SelectedSplit = "RunFinish";//this value gets used by the generic _finishHistogram and also Hypotheth/Index to distinguish the Divs

            return PartialView("_GivenRunRangeFinish", modelView);
        }


        public PartialViewResult DisplayFinishHistogram(SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel();
            filter.SaveRaceFilterValues(model);
            var viewModel = GetEstimatedTime(filter);
            return PartialView("_FinishHistogram", viewModel);
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
            modelView.Triathletes = athletes;
            modelView.Stats.Add(GetStats(athletes, filter.AvailableRaces.Single(r => r.RaceId == raceId)));
            modelView.SelectedSkillLevel = filter.SkillLevel; //TO-DO: fix this up so it removes the redundancy
       
           

            return modelView;
        }

        #endregion

    }
}
﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using RaceAnalysis.DAL;
using X.PagedList;

namespace RaceAnalysis.Controllers
{
    public abstract class BaseController : Controller
    {
        protected RaceAnalysisDbContext _DBContext = new RaceAnalysisDbContext();
        protected TriathletesDAL _DAL; 

        public BaseController()
        {
            _DAL = new TriathletesDAL(_DBContext);
        }

        
        public ActionResult ShowFilterByRace(string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel(_DBContext);
            viewmodel.SaveRaceFilterValues(races,agegroups,genders);

            return PartialView("~/Views/Shared/_RaceFilter.cshtml", viewmodel);
        }
        public ActionResult ShowFilterByDuration(string races, string agegroups, string genders)
        {
            var viewmodel = new TriStatsViewModel();
            viewmodel.Filter = new RaceFilterViewModel(_DBContext);
            viewmodel.Filter.SaveRaceFilterValues(races, agegroups, genders);

            return PartialView("~/Views/Shared/_SearchByDuration.cshtml", viewmodel);
        }

        [HttpPost]
        public ActionResult SearchByAthleteName(FormCollection form, string races, string agegroups, string genders)
        {
            return SearchOpenFieldQuery("name", form["SearchString"], races, agegroups, genders);
        }


        [HttpPost]
        public ActionResult SearchByTimeThresholds(FormCollection form, string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel(_DBContext);
            viewmodel.SaveRaceFilterValues(races, agegroups, genders);

            var swimMinsLow = Convert.ToInt16(form["swim-lowtime-value"].ZeroIfEmpty());
            var swimLow = new TimeSpan(0, swimMinsLow, 0);

            var swimMinsHigh = Convert.ToInt16(form["swim-hightime-value"].MaxIfEmpty());
            var swimHigh = new TimeSpan(0, swimMinsHigh, 0);

            var bikeMinsLow = Convert.ToInt16(form["bike-lowtime-value"].ZeroIfEmpty());
            var bikeLow = new TimeSpan(0, bikeMinsLow, 0);

            var bikeMinsHigh = Convert.ToInt16(form["bike-hightime-value"].MaxIfEmpty());
            var bikeHigh = new TimeSpan(0, bikeMinsHigh, 0);
            
            var runMinsLow = Convert.ToInt16(form["run-lowtime-value"].ZeroIfEmpty());
            var runLow = new TimeSpan(0, runMinsLow, 0);

            var runMinsHigh = Convert.ToInt16(form["run-hightime-value"].MaxIfEmpty());
            var runHigh = new TimeSpan(0, runMinsHigh, 0);

            var finishMinsLow = Convert.ToInt16(form["finish-lowtime-value"].ZeroIfEmpty());
            var finishLow = new TimeSpan(0, finishMinsLow, 0);

            var finishMinsHigh = Convert.ToInt16(form["finish-hightime-value"].MaxIfEmpty());
            var finishHigh = new TimeSpan(0, finishMinsHigh, 0);


            ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);

            var athletes = search.SearchByDuration(swimLow, swimHigh, bikeLow, bikeHigh, runLow, runHigh, finishLow,finishHigh,viewmodel);
            return DisplayResultsView(athletes,viewmodel);
        }
        private ActionResult SearchOpenFieldQuery(string field, string query, string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel(_DBContext);
            viewmodel.SaveRaceFilterValues(races, agegroups, genders);

            ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);

            var athletes = search.SearchFieldQuery(field, query);

            return DisplayResultsView(athletes,viewmodel);
        }

        //called from the racefilter
        public ActionResult SelectedRaces(PostedFilterValues postedValues)
        {
            var filter = new RaceFilterViewModel(_DBContext);
            filter.SaveRaceFilterValues(postedValues);

            int[] raceIds = postedValues.RaceIds != null ?
                Array.ConvertAll(postedValues.RaceIds, element => Convert.ToInt32(element)) :
                 new int[] { };


            int[] agegroupIds = postedValues.AgeGroupIds != null ?
                Array.ConvertAll(postedValues.AgeGroupIds, element => Convert.ToInt32(element)) :
                new int[] { };


            int[] genderIds = postedValues.GenderIds != null ?
                Array.ConvertAll(postedValues.GenderIds, element => Convert.ToInt32(element)) :
                new int[] { };


            return DisplayResultsView( 1, raceIds, agegroupIds, genderIds);

        }
        //called from the Paging Control
        //this is the best way I found to pass the arrays from view to controller
        public ActionResult DisplayPagedAthletes(int page, string races, string agegroups, string genders)
        {
            var r = Array.ConvertAll(races.Split(','), int.Parse);
            var a = Array.ConvertAll(agegroups.Split(','), int.Parse);
            var g = Array.ConvertAll(genders.Split(','), int.Parse);

            return DisplayResultsView(page, r, a, g);
        }



        #region Protected Methods
        protected abstract ActionResult DisplayResultsView(int page, int[] races, int[] agegroups, int[] genders);

        protected virtual ActionResult DisplayResultsView(List<Triathlete> athletes, RaceFilterViewModel filter)
        {
            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = filter;
            viewmodel.Triathletes = athletes.ToPagedList(pageNumber: 1, pageSize: 100); //max xx per page

            return View("List", viewmodel);
        }

        protected bool GetTrueOrFalseFromCheckbox(string val)
        {
            string[] values = val.Split(',');
            if (values != null && values.Length > 1)
                return true;
            else
                return false;

        }


   


        #endregion //Protected Methods

        #region Private Methods
      
      
      
    


        #endregion//Private Methods

    }
}
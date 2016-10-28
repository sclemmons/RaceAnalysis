﻿using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    public class TriStatsAgeGroupsCompareController : TriStatsController
    {
        public ActionResult Compare()
        {
            var viewmodel = new AgeGroupCompareViewModel();
            viewmodel.Filter = new RaceFilterViewModel();
            return View(viewmodel);
        }

        protected override ActionResult DisplayResultsView(int page, int[] raceIds, int[] agegroupIds, int[] genderIds)
        {
            var viewModel = new AgeGroupCompareViewModel();
            viewModel.Filter = new RaceFilterViewModel();
            viewModel.Filter.SaveRaceFilterValues(raceIds, agegroupIds, genderIds);
            var athletes = new List<Triathlete>();

            //for now let's only deal with a single race to keep it simple
            int raceId = viewModel.Filter.SelectedRaces.First().RaceId;


            //pulling from selected age groups so that we can doo the same when we draw the chart
            foreach (var ag in viewModel.Filter.SelectedAgeGroups) //collect the stats for each age group
            {
                var athletesPerAG = _DAL.GetAthletes(raceIds, new int[] {ag.AgeGroupId}, genderIds);
                athletes.AddRange(athletesPerAG);
                viewModel.Stats.Add(GetStats(athletesPerAG, _DBContext.Races.Single(r => r.RaceId == raceId)));
            }

            return View("Compare", viewModel);
        }

       
    }
}
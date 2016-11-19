﻿using RaceAnalysis.Models;
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
            var viewModel = new TriathletesViewModel();
            viewModel.Filter = filter;

            var allAthletes = new List<Triathlete>();
            foreach (int raceId in filter.SelectedRaceIds) 
            {

                var athletes = _RaceService.GetAthletes(
                      new BasicRaceCriteria
                      {
                          SelectedRaceIds = new int[] { raceId },
                          SelectedAgeGroupIds = AgeGroup.Expand(filter.SelectedAgeGroupIds),
                          SelectedGenderIds = filter.SelectedGenderIds
                      },
                      filter
                );
                allAthletes.AddRange(athletes);
            }

            viewModel.Triathletes = allAthletes;
            return View("Compare", viewModel);
        }
    }
}
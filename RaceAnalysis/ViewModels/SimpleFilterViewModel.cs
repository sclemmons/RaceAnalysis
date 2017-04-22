﻿using RaceAnalysis.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{

    public interface ISimpleDurationFilter
    {

        string swimlowtimevalue { get; set; }
        string swimhightimevalue { get; set; }

        string bikelowtimevalue { get; set; }
        string bikehightimevalue { get; set; }

        string runlowtimevalue { get; set; }
        string runhightimevalue { get; set; }

        string finishlowtimevalue { get; set; }
        string finishhightimevalue { get; set; }
    }
    public interface ISimpleRaceFilter
    {
        string Races { get; set; }
        string AgeGroups { get; set; }
        string Genders { get; set; }
    }

    public interface IComplexRaceFilter
    {
        string[] selectedRaceIds { get; set; }
        int[] selectedAgeGroupIds { get; set; }
        int[] selectedGenderIds { get; set; }
    }

    /// <summary>
    /// FilterViewModel - this model can be used in an HTTP Post
    /// </summary>
    public class FilterViewModel : IComplexRaceFilter, ISimpleDurationFilter
    {
        public string[] selectedRaceIds { get; set; }
        public int[] selectedAgeGroupIds { get; set; }
        public int[] selectedGenderIds { get; set; }

        public string swimlowtimevalue { get; set; }
        public string swimhightimevalue { get; set; }

        public string bikelowtimevalue { get; set; }
        public string bikehightimevalue { get; set; }

        public string runlowtimevalue { get; set; }
        public string runhightimevalue { get; set; }

        public string finishlowtimevalue { get; set; }
        public string finishhightimevalue { get; set; }

        public string distance { get; set; }
}

    /// <summary>
    /// SimpleFilterViewModel - all properties are strings so it can be used with an HTTP GET
    /// </summary>
    public class SimpleFilterViewModel : ISimpleRaceFilter, ISimpleDurationFilter
    {
        public string Races { get; set; }
        public string AgeGroups { get; set; }
        public string Genders { get; set; }

        public string swimlowtimevalue { get; set; }
        public string swimhightimevalue { get; set; }

        public string bikelowtimevalue { get; set; }
        public string bikehightimevalue { get; set; }

        public string runlowtimevalue { get; set; }
        public string runhightimevalue { get; set; }

        public string finishlowtimevalue { get; set; }
        public string finishhightimevalue { get; set; }

        public string selectedAthletes { get; set; }

        public string distance { get; set; }

        public string skilllevel { get; set; }


        public void ClearDuration()
        {
            finishlowtimevalue = null;
            finishhightimevalue = null;
            swimlowtimevalue = null;
            swimhightimevalue = null;
            bikelowtimevalue = null;
            bikehightimevalue = null;
            runlowtimevalue = null;
            runhightimevalue = null;
        }

        
        public static SimpleFilterViewModel Create(FilterViewModel filterView)
        {
            var simple = new SimpleFilterViewModel();

            simple.Races = filterView.selectedRaceIds.JoinIfNotNull();
            simple.AgeGroups = filterView.selectedAgeGroupIds.JoinIfNotNull();
            simple.Genders = filterView.selectedGenderIds.JoinIfNotNull();

            simple.swimlowtimevalue = filterView.swimlowtimevalue;
            simple.swimhightimevalue = filterView.swimhightimevalue;

            simple.bikelowtimevalue = filterView.bikelowtimevalue;
            simple.bikehightimevalue = filterView.bikehightimevalue;

            simple.runlowtimevalue = filterView.bikelowtimevalue;
            simple.runhightimevalue = filterView.bikelowtimevalue;

            simple.finishlowtimevalue = filterView.bikelowtimevalue;
            simple.finishhightimevalue = filterView.bikehightimevalue;

            simple.selectedAthletes = null; //no conversion;

            simple.skilllevel = null; //no conversion

            simple.distance = filterView.distance;

            return simple;
    }

}
}

﻿using RaceAnalysis.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysis.Models
{
    public class RaceFilterViewModel : IRaceCriteria,IDurationFilter
    {
        private RaceAnalysisDbContext _DBContext;
        public RaceFilterViewModel()
        {
            _DBContext = new RaceAnalysisDbContext();
            PopulateRaceFilter();
        }
        public RaceFilterViewModel(RaceAnalysisDbContext db)
        {
            _DBContext = db;
            PopulateRaceFilter();
        }
        public IList<Race> AvailableRaces { get; set; }
        public IList<int> SelectedRaceIds { get; set; }
      

        public IList<AgeGroup> AvailableAgeGroups { get; set; }
        public IList<int> SelectedAgeGroupIds { get; set; }


        public IList<Gender> AvailableGenders { get; set; }
        public IList<int> SelectedGenderIds { get; set; }


        public TimeSpan SwimLow { get; set; }
        public TimeSpan SwimHigh { get; set; }
        public TimeSpan BikeLow { get; set; }
        public TimeSpan BikeHigh { get; set; }
        public TimeSpan RunLow { get; set; }
        public TimeSpan RunHigh { get; set; }
        public TimeSpan FinishLow { get; set; }
        public TimeSpan FinishHigh { get; set; }
        

        private void PopulateRaceFilter()
        {
           
            AvailableRaces = _DBContext.Races.ToList();
            AvailableAgeGroups = _DBContext.AgeGroups.ToList();
            AvailableGenders = _DBContext.Genders.ToList();

            if (SelectedRaceIds == null)
            { SelectedRaceIds = new List<int>(); }

            if (SelectedAgeGroupIds == null)
            { SelectedAgeGroupIds = GetDefaultAgeGroups(); }

            if (SelectedGenderIds == null)
                { SelectedGenderIds = GetDefaultGenders(); }

            SwimLow = new TimeSpan(0, 0, 0);
            SwimHigh = new TimeSpan(3, 0, 0);
            BikeLow = new TimeSpan(3, 0, 0);
            BikeHigh = new TimeSpan(10, 0, 0);
            RunLow = new TimeSpan(2, 0, 0);
            RunHigh = new TimeSpan(7, 0, 0);
            FinishLow = new TimeSpan(8, 0, 0);
            FinishHigh = new TimeSpan(17, 0, 0);


        }
        public void SaveRaceFilterValues(FilterViewModel model)
        {
            SaveRaceFilterValues(((IComplexRaceFilter)model));
            SaveDurationValues(model);
        }
        public void SaveRaceFilterValues(SimpleFilterViewModel model)
        {
            SaveRaceFilterValues(model.Races, model.AgeGroups, model.Genders);
            SaveDurationValues(model);
        }
        
        private void SaveDurationValues(ISimpleDurationFilter filter)
        {
         
            if (filter.swimlowtimevalue != null)
                SwimLow = new TimeSpan(0,Int32.Parse(filter.swimlowtimevalue),0);

            if (filter.swimhightimevalue != null)
                SwimHigh = new TimeSpan(0, Int32.Parse(filter.swimhightimevalue), 0);

            if (filter.bikelowtimevalue != null)
                BikeLow = new TimeSpan(0, Int32.Parse(filter.bikelowtimevalue), 0);

            if (filter.bikehightimevalue != null)
                BikeHigh = new TimeSpan(0, Int32.Parse(filter.bikehightimevalue), 0);
            
            if (filter.runlowtimevalue != null)
                RunLow = new TimeSpan(0, Int32.Parse(filter.runlowtimevalue), 0);

            if (filter.runhightimevalue != null)
                RunHigh = new TimeSpan(0, Int32.Parse(filter.runhightimevalue), 0);


            if (filter.finishlowtimevalue != null)
                FinishLow = new TimeSpan(0, Int32.Parse(filter.finishlowtimevalue), 0);

            if (filter.finishhightimevalue != null)
                FinishHigh = new TimeSpan(0, Int32.Parse(filter.finishhightimevalue), 0);

        }

        private void SaveRaceFilterValues(IComplexRaceFilter filter)
        {
           
            PopulateRaceFilter(); 

            //TODO: Look at this. seems overly complex

            SelectedRaceIds = filter.selectedRaceIds == null ? _DBContext.Races.Select(s => s.RaceId).ToList()
             : _DBContext.Races.Where(r => filter.selectedRaceIds.Any(x => r.RaceId == x)).Select(s=>s.RaceId).ToList();


            SelectedAgeGroupIds = filter.selectedAgeGroupIds == null ? _DBContext.AgeGroups.Select(s => s.AgeGroupId).ToList()
                : _DBContext.AgeGroups.Where(r => filter.selectedAgeGroupIds.Any(x => r.AgeGroupId == x)).Select(s=>s.AgeGroupId).ToList();



            SelectedGenderIds = filter.selectedGenderIds == null ? _DBContext.Genders.Select(s=>s.GenderId).ToList()
                : _DBContext.Genders.Where(r => filter.selectedGenderIds.Any(x => r.GenderId == x)).Select(s=>s.GenderId).ToList();


            
        }
        public void SaveRaceFilterValues(string races, string agegroups, string genders)
        {
            SaveRaceFilterValues((IComplexRaceFilter)
                   new FilterViewModel
                   {
                       selectedRaceIds = Array.ConvertAll(races.ZeroIfEmpty().Split(','), int.Parse),
                       selectedAgeGroupIds = Array.ConvertAll(agegroups.ZeroIfEmpty().Split(','), int.Parse),
                       selectedGenderIds = Array.ConvertAll(genders.ZeroIfEmpty().Split(','), int.Parse)
                   });
           
        }

        public List<int> GetDefaultGenders()
        {

            var query = _DBContext.Genders
                              .Where(t => t.Value == "M"
                                    || t.Value == "F").Select(t =>t.GenderId);

            return query.ToList();

        }
        public List<int> GetDefaultAgeGroups()
        {
       //for testing purposes i'm only enabling one by default
            var query = _DBContext.AgeGroups
                .Where(t =>
                /*
                        t.Value == "18-24" ||
                        t.Value == "25-29" ||
                        t.Value == "30-34" ||
                        t.Value == "40-44" ||
                        t.Value == "45-49" ||
                 */
                        t.Value == "50-54" 
              /*****          
                        t.Value == "55-59" ||
                        t.Value == "60-64" ||
                        t.Value == "65-69" ||
                        t.Value == "70-74" ||
                        t.Value == "75-79" ||
                        t.Value == "80-84" ||
                        t.Value == "85-89" ||
                        t.Value == "90+Plus" ||
                        t.Value == "PC"
                ***/        
                ).Select(t=> t.AgeGroupId); 

            return query.ToList();

        }

        public List<int> GetRequestIds(RaceFilterViewModel filter)
        {

            int[] raceIds = filter.SelectedRaceIds.ToArray();
            int[] ageGroupIds = filter.SelectedAgeGroupIds.ToArray();
            int[] genderIds = filter.SelectedGenderIds.ToArray();

            var query = _DBContext.RequestContext
                             .Where(r => raceIds.Contains(r.RaceId))
                             .Where(r => ageGroupIds.Contains(r.AgeGroupId))
                             .Where(r => genderIds.Contains(r.GenderId))
                             .Select(r => r.RequestContextId);


            return query.ToList();

        }


    }
    
}
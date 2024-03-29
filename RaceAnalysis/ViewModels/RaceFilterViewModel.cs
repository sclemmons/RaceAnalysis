﻿using RaceAnalysis.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysis.Models
{
    public class RaceFilterViewModel : IRaceCriteria, IDurationFilter
    {

        public RaceFilterViewModel()
        {
            PopulateRaceFilter();
        }
        public RaceFilterViewModel(string distance, string sortOrder=null)
        {
            PopulateRaceFilter(distance,sortOrder);
        }
        public RaceFilterViewModel(SimpleFilterViewModel model, string sortOrder = null)
        {
            PopulateRaceFilter(model.distance,sortOrder);
            SaveRaceFilterValues(model);

        }
        public RaceFilterViewModel(FilterViewModel model)
        {
            PopulateRaceFilter(model.distance);
            SaveRaceFilterValues(model);
        }
     

        public IEnumerable<Race> AvailableRaces { get; set; }
        public IList<string> SelectedRaceIds { get; set; }

        //for ajax search call
        public string SelectedRaceId { get; set; }

        public string SelectedRaceNames
        {
            get
            {
                return String.Join(",",
                  AvailableRaces.Where(r => SelectedRaceIds.Contains(r.RaceId)).Select(r => r.DisplayName).ToList());
       
            }
            
        }

        public IList<AgeGroup> AvailableAgeGroups { get; set; }
        public IList<int> SelectedAgeGroupIds { get; set; }


        public IList<Gender> AvailableGenders { get; set; }
        public IList<int> SelectedGenderIds { get; set; }

        public string SkillLevel { get; set; } //TO-DO: remove this and other properties into separate classes/interfaces


        public TimeSpan SwimLow { get; set; }
        public TimeSpan SwimHigh { get; set; }
        public TimeSpan BikeLow { get; set; }
        public TimeSpan BikeHigh { get; set; }
        public TimeSpan RunLow { get; set; }
        public TimeSpan RunHigh { get; set; }
        public TimeSpan FinishLow { get; set; }
        public TimeSpan FinishHigh { get; set; }

        public IList<int> SelectedAthleteIds { get; set; }

        public string Distance { get; private set; }
        public string Sort { get; private set; }
        private void PopulateRaceFilter(string distance=null,string sortOrder=null)
        {
            PopulateRaces(distance, sortOrder);
            PopulateAgeGroupsAndGenders();

            if (SelectedRaceIds == null)
            { SelectedRaceIds = new List<string>(); }

            if (SelectedAgeGroupIds == null)
            { SelectedAgeGroupIds = GetDefaultAgeGroups(); }

            if (SelectedGenderIds == null)
            { SelectedGenderIds = GetDefaultGenders(); }

            SwimLow = new TimeSpan(0, 0, 0);
            SwimHigh = new TimeSpan(3, 0, 0);
            BikeLow = new TimeSpan(0, 0, 0);
            BikeHigh = new TimeSpan(10, 0, 0);
            RunLow = new TimeSpan(0, 0, 0);
            RunHigh = new TimeSpan(8, 0, 0);
            FinishLow = new TimeSpan(1, 0, 0);
            FinishHigh = new TimeSpan(17, 0, 0);

            if (SelectedAthleteIds == null)
            { SelectedAthleteIds = new List<int>(); }


        }
        private void PopulateAgeGroupsAndGenders()
        {
            using (var db = new RaceAnalysisDbContext())
            {
                AvailableAgeGroups = GetAvailableAgeGroups(db);
                AvailableGenders = db.Genders.ToList();
            }
        } 
        private void PopulateRaces(string distance, string sortOrder)
        {
            switch (distance)
            {
                case "140.6":
                    distance = "140.6";
                    break;
                case "70.3":
                    distance = "70.3";
                    break;
                default:
                    distance = null;
                    break;
            }
            Distance = distance;

            IQueryable<Race> races;
            using (var db = new RaceAnalysisDbContext())
            {
                //note: need to include the Conditions info because the AvailableRaces property is being 
                //used to populate the Stats. Don't know if 
                //if I like this choice but going with it for now. Other option is to populate the stats using
                //available races from the dbContext. 
                races = db.Races.Include("Conditions");
                
                if (!String.IsNullOrEmpty(distance))
                    races = races.Where(r => r.Distance == distance);



                switch (sortOrder)
                {
                    case "name_desc":
                        races = races.OrderByDescending(r => r.LongDisplayName);
                        break;
                    case "Date":
                        races = races.OrderBy(r => r.RaceDate);
                        break;
                    case "date_desc":
                        races = races.OrderByDescending(r => r.RaceDate);
                        break;
                    default:  // Name ascending 
                        races = races.OrderBy(r => r.LongDisplayName);
                        break;
                }
                Sort = sortOrder;

                foreach (var r in races)
                {
                    r.RaceId = r.RaceId.ToUpper(); //this way we only do it here and not everywhere in the code 
                }

                AvailableRaces = races.ToList();
            }
        }

        private void SaveRaceFilterValues(FilterViewModel model)
        {
            SaveRaceFilterValues(((IComplexRaceFilter)model));
            SaveDurationValues(model);
        }


        private void SaveRaceFilterValues(SimpleFilterViewModel model)
        {
            SaveRaceFilterValues(model.Races, model.AgeGroups, model.Genders);
            SaveDurationValues(model);
            SaveSelectedAthletes(model);
            SaveSelectedSkillLevel(model);
            SaveDistance(model);
            
        }

        private void SaveDistance(SimpleFilterViewModel model)
        {
            Distance = model.distance;

        }
        private void SaveSelectedSkillLevel(SimpleFilterViewModel model)
        {
            SkillLevel = model.skilllevel;
        }

        private void SaveSelectedAthletes(SimpleFilterViewModel model)
        {
            if(model.selectedAthletes != null)
                SelectedAthleteIds = Array.ConvertAll(model.selectedAthletes.Split(','), int.Parse);
        }
        private void SaveDurationValues(ISimpleDurationFilter filter)
        {

            if (filter.swimlowtimevalue != null)
                SwimLow = new TimeSpan(0, Int32.Parse(filter.swimlowtimevalue), 0);

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

            SelectedRaceIds = filter.selectedRaceIds.Select(s => s.ToUpper()).ToList();
            SelectedAgeGroupIds = filter.selectedAgeGroupIds == null ? new int[] { 0 } : filter.selectedAgeGroupIds;
            SelectedGenderIds = filter.selectedGenderIds == null ? new int[] { 0 } : filter.selectedGenderIds;
            
        }
        public void SaveRaceFilterValues(string races, string agegroups, string genders)
        {
            SaveRaceFilterValues((IComplexRaceFilter)
                   new FilterViewModel
                   {
                       selectedRaceIds = races.EmptyIfNull().Split(','),
                       selectedAgeGroupIds = Array.ConvertAll(agegroups.ZeroIfEmpty().Split(','), int.Parse),
                       selectedGenderIds = Array.ConvertAll(genders.ZeroIfEmpty().Split(','), int.Parse),
                      
                   });

        }

        private List<int> GetDefaultGenders()
        {
            using (var db = new RaceAnalysisDbContext())
            {
                var query = db.Genders
                                  .Where(t => t.Value == "M"
                                        || t.Value == "F").Select(t => t.GenderId);

                return query.ToList();
            }
        }

        private List<AgeGroup> GetAvailableAgeGroups(RaceAnalysisDbContext db)
        {
            var ag = db.AgeGroups.OrderBy(a => a.DisplayOrder).ToList();
            return ag;
        }
        private List<int> GetDefaultAgeGroups()
        {
            return new int[] { 0 }.ToList(); //all age-groupers
        }


    }

}
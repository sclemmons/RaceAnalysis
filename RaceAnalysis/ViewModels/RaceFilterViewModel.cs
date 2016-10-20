using RaceAnalysis.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class RaceFilterViewModel
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
        public IList<Race> SelectedRaces { get; set; }
        public PostedFilterValues PostedValues { get; set; }

        public IList<AgeGroup> AvailableAgeGroups { get; set; }
        public IList<AgeGroup> SelectedAgeGroups { get; set; }


        public IList<Gender> AvailableGenders { get; set; }
        public IList<Gender> SelectedGenders { get; set; }




        private  void PopulateRaceFilter()
        {
           
            AvailableRaces = _DBContext.Races.ToList();
            AvailableAgeGroups = _DBContext.AgeGroups.ToList();
            AvailableGenders = _DBContext.Genders.ToList();

            if (SelectedRaces == null)
            { SelectedRaces = new List<Race>(); }

            if (SelectedAgeGroups == null)
            { SelectedAgeGroups = GetDefaultAgeGroups(); }

            if (SelectedGenders == null)
                { SelectedGenders = GetDefaultGenders(); }


         
        }
        public void SaveRaceFilterValues(PostedFilterValues postedValues)
        {

            PostedValues = postedValues;

            SelectedRaces = postedValues.RaceIds == null ? _DBContext.Races.ToList()
                : _DBContext.Races.Where(r => postedValues.RaceIds.Any(x => r.RaceId.ToString().Equals(x))).ToList();


            SelectedAgeGroups = postedValues.AgeGroupIds == null ? _DBContext.AgeGroups.ToList()
                : _DBContext.AgeGroups.Where(r => postedValues.AgeGroupIds.Any(x => r.AgeGroupId.ToString().Equals(x))).ToList();


            SelectedGenders = postedValues.GenderIds == null ? _DBContext.Genders.ToList()
                : _DBContext.Genders.Where(r => postedValues.GenderIds.Any(x => r.GenderId.ToString().Equals(x))).ToList();


        }
        public void SaveRaceFilterValues(int[] raceIds, int[] ageGroupIds, int[] genderIds)
        {
           
            PopulateRaceFilter(); //don't know if I need this..... TODO: look at this

            SelectedRaces = raceIds == null ? _DBContext.Races.ToList()
             : _DBContext.Races.Where(r => raceIds.Any(x => r.RaceId == x)).ToList();


            SelectedAgeGroups = ageGroupIds == null ? _DBContext.AgeGroups.ToList()
                : _DBContext.AgeGroups.Where(r => ageGroupIds.Any(x => r.AgeGroupId == x)).ToList();



            SelectedGenders = genderIds == null ? _DBContext.Genders.ToList()
                : _DBContext.Genders.Where(r => genderIds.Any(x => r.GenderId == x)).ToList();



        }
        public void SaveRaceFilterValues(string races,string agegroups, string genders)
        {
            var r = Array.ConvertAll(races.ZeroIfEmpty().Split(','), int.Parse);
            var a = Array.ConvertAll(agegroups.ZeroIfEmpty().Split(','), int.Parse);
            var g = Array.ConvertAll(genders.ZeroIfEmpty().Split(','), int.Parse);
            SaveRaceFilterValues(r, a, g);
        }

        public List<Gender> GetDefaultGenders()
        {

            var query = _DBContext.Genders
                              .Where(t => t.Value == "M"
                                    || t.Value == "F");

            return query.ToList();

        }
        public List<AgeGroup> GetDefaultAgeGroups()
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
                ); 

            return query.ToList();

        }

        public List<int> GetRequestIds(RaceFilterViewModel filter)
        {

            int[] raceIds = filter.SelectedRaces.Select(n => n.RaceId).ToArray();
            int[] ageGroupIds = filter.SelectedAgeGroups.Select(n => n.AgeGroupId).ToArray();
            int[] genderIds = filter.SelectedGenders.Select(n => n.GenderId).ToArray();

            var query = _DBContext.RequestContext
                             .Where(r => raceIds.Contains(r.RaceId))
                             .Where(r => ageGroupIds.Contains(r.AgeGroupId))
                             .Where(r => genderIds.Contains(r.GenderId))
                             .Select(r => r.RequestContextId);


            return query.ToList();

        }


    }
    public class PostedFilterValues
    {
        public string[] RaceIds { get; set; }
        public string[] AgeGroupIds { get; set; }
        public string[] GenderIds { get; set; }

    }
}
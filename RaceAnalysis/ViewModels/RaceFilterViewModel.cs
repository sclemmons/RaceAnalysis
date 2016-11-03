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
        public IList<int> SelectedRaceIds { get; set; }
        public PostedFilterValues PostedValues { get; set; }

        public IList<AgeGroup> AvailableAgeGroups { get; set; }
        public IList<int> SelectedAgeGroupIds { get; set; }


        public IList<Gender> AvailableGenders { get; set; }
        public IList<int> SelectedGenderIds { get; set; }




        private  void PopulateRaceFilter()
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


         
        }
        
        public void SaveRaceFilterValues(int[] raceIds, int[] ageGroupIds, int[] genderIds)
        {
           
            PopulateRaceFilter(); //don't know if I need this..... TODO: look at this

            SelectedRaceIds = raceIds == null ? _DBContext.Races.Select(s => s.RaceId).ToList()
             : _DBContext.Races.Where(r => raceIds.Any(x => r.RaceId == x)).Select(s=>s.RaceId).ToList();


            SelectedAgeGroupIds = ageGroupIds == null ? _DBContext.AgeGroups.Select(s => s.AgeGroupId).ToList()
                : _DBContext.AgeGroups.Where(r => ageGroupIds.Any(x => r.AgeGroupId == x)).Select(s=>s.AgeGroupId).ToList();



            SelectedGenderIds = genderIds == null ? _DBContext.Genders.Select(s=>s.GenderId).ToList()
                : _DBContext.Genders.Where(r => genderIds.Any(x => r.GenderId == x)).Select(s=>s.GenderId).ToList();



        }
        public void SaveRaceFilterValues(string races,string agegroups, string genders)
        {
            var r = Array.ConvertAll(races.ZeroIfEmpty().Split(','), int.Parse);
            var a = Array.ConvertAll(agegroups.ZeroIfEmpty().Split(','), int.Parse);
            var g = Array.ConvertAll(genders.ZeroIfEmpty().Split(','), int.Parse);
            SaveRaceFilterValues(r, a, g);
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
    public class PostedFilterValues
    {
        public string[] RaceIds { get; set; }
        public string[] AgeGroupIds { get; set; }
        public string[] GenderIds { get; set; }

    }
}
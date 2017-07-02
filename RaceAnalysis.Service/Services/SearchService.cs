using RaceAnalysis.Models;
using RaceAnalysis.Shared.DAL;
using RaceAnalysis.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RaceAnalysis.Service
{
    public class SearchService : ISearchService
    {

        public List<ShallowTriathlete> SearchAthletesByName(string name, string[] raceIds = null)
        {
            List<ShallowTriathlete> results;
            var search = FtsInterceptor.Fts(name.Trim().ToLower());
            using (var db = new RaceAnalysisDbContext())
            {

                var triathletes = db.Triathletes.Include("RequestContext.RaceId");
                var shallow = triathletes.OrderBy(t => t.Name).Select(t => new ShallowTriathlete()
                { Name = t.Name, Id = t.TriathleteId, RaceId = t.RequestContext.RaceId }).
                    Where(t => t.Name.Contains(search)).Take(30);


                if (raceIds != null)
                    shallow = shallow.Where(a => raceIds.Contains(a.RaceId));

                results = shallow.ToList();
            }

            return results.Distinct(new ShallowTriathleteComparer()).ToList();

        }

        public List<ShallowRace> SearchRacesByName(string name)
        {
            List<ShallowRace> results;
            var search = FtsInterceptor.Fts(name.Trim().ToLower());

            List<Race> foundRaces;

            using (var db = new RaceAnalysisDbContext())
            {
                foundRaces = db.Races.
                    Where(r => r.LongDisplayName.Contains(search))
                    .OrderBy(r => r.LongDisplayName).ToList();
                
          //      var results = shallow.Select(r => new ShallowRace()
          //          { Name = r.RaceCategoryName, Id = r.ShortName });

               
            }

            return foundRaces.Select(r => new ShallowRace() { Name = r.RaceCategoryName, Id = r.ShortName })
                           .Distinct(new ShallowRaceComparer()).ToList();

         //   if (!shallowRaces.Contains(race, new ShallowRaceComparer()))
           //     shallowRaces.Add(race);
          
        }

    }
}

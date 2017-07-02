using RaceAnalysis.Models;
using System.Collections.Generic;

namespace RaceAnalysis.Service.Interfaces
{
    public interface ISearchService
    {
        List<ShallowTriathlete> SearchAthletesByName(string name, string[] raceIds = null);
        List<ShallowRace> SearchRacesByName(string name);

    }
}

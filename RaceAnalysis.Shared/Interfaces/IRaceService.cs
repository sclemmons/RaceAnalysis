using RaceAnalysis.Models;
using System;
using System.Collections.Generic;


namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceService
    {
        List<Triathlete> GetAthletes(IRaceCriteria criteria);
        List<Triathlete> GetAthletes(IRaceCriteria criteria, IDurationFilter filter);
        List<Triathlete> GetAthletesFromStorage(IRaceCriteria criteria); //for testing purposes
        List<Triathlete> GetAthletesFromSource(IRaceCriteria criteria); //for testing purposes

        Triathlete GetAthleteById(int id);

        List<Race> GetRacesBySwimCondition(string conditions);
        List<Race> GetRacesByBikeCondition(string conditions);
        List<Race> GetRacesByRunCondition(string conditions);

        List<ShallowTriathlete> GetShallowAthletesByName(string name,string[] raceIds=null);
        List<Triathlete> GetAthletesByName(string name, string[] raceIds = null);



        void ReIndex(); //note we may move this



        List<Race> GetRacesByTagId(List<int> tagIds);
    }

}

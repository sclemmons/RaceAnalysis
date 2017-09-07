using RaceAnalysis.Models;
using System;
using System.Collections.Generic;


namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceService
    {
        List<Triathlete> GetAthletes(IRaceCriteria criteria,bool useCache=true);
        List<Triathlete> GetAthletes(IRaceCriteria criteria, IDurationFilter filter);
        List<Triathlete> GetAthletesFromStorage(IRaceCriteria criteria); //for testing purposes
        List<Triathlete> GetAthletesFromSource(IRaceCriteria criteria); 

        void VerifyRequestContext(RequestContext context);
        void VerifyRace(Race race);


        Triathlete GetAthleteById(int id);

        List<Race> GetRacesBySwimCondition(string conditions);
        List<Race> GetRacesByBikeCondition(string conditions);
        List<Race> GetRacesByRunCondition(string conditions);

        List<Triathlete> GetAthletesByName(string name, string[] raceIds = null);



        void ReIndex(); //note we may move this



        List<Race> GetRacesByTagId(List<int> tagIds);
        List<Race> GetRacesById(string id);
        List<Race> GetRacesByGroupName(string name);
        List<Race> GetRacesMostRecent(int count);
        List<RaceAggregate> GetRacesFastestSwim(int count,string distance);
        List<RaceAggregate> GetRacesFastestBike(int count, string distance);
        List<RaceAggregate> GetRacesFastestRun(int count, string distance);
        List<RaceAggregate> GetRacesFastestFinish(int count, string distance);

    }

}

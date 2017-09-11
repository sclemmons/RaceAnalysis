using RaceAnalysis.Models;
using System;
using System.Collections.Generic;


namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceService
    {
        /*athlete info*/
        Triathlete GetAthleteById(int id);
        List<Triathlete> GetAthletes(IRaceCriteria criteria,bool useCache=true);
        List<Triathlete> GetAthletes(IRaceCriteria criteria, IDurationFilter filter);
        List<Triathlete> GetAthletesFromStorage(IRaceCriteria criteria); //for testing purposes
        List<Triathlete> GetAthletesFromSource(IRaceCriteria criteria);
        List<Triathlete> GetAthletesFastestFinish(int count,string distance,string genderValue=null);
        List<Triathlete> GetAthletesByName(string name, string[] raceIds = null);
        

        void VerifyRequestContext(RequestContext context);
        void VerifyRace(Race race);

        void ReIndex(); //note we may move this


        /*race info */
        List<Race> GetRacesBySwimCondition(string conditions);
        List<Race> GetRacesByBikeCondition(string conditions);
        List<Race> GetRacesByRunCondition(string conditions);

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

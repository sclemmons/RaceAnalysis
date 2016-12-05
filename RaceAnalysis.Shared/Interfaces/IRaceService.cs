using RaceAnalysis.Models;
using System;
using System.Collections.Generic;


namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceService
    {
        List<Triathlete> GetAthletes(IRaceCriteria criteria);

        List<Triathlete> GetAthletes(IRaceCriteria criteria, IDurationFilter filter);

        List<Race> GetRacesByCondition(string conditions);

        List<Triathlete> GetAthletesByName(string name);
            
        void ReIndex(); //note we may move this


    }

}

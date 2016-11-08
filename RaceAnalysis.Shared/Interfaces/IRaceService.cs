using RaceAnalysis.Models;
using System.Collections.Generic;


namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceService
    {
        List<Triathlete> GetAthletes(IRaceCriteria criteria);

        List<Triathlete> GetAthletes(IRaceCriteria criteria, IDurationFilter filter);


    }

}

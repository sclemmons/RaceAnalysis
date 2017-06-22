using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Service.Interfaces
{
    public interface ICacheService
    {
        void PopulateCache(List<ShallowTriathlete> athletes);
        List<ShallowTriathlete> GetShallowAthletes();
        void FlushShallowAthletes();

    }
}

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
        Task PopulateCacheAsync(List<ShallowTriathlete> athletes);
        List<ShallowTriathlete> GetShallowAthletes(string name);
        void FlushShallowAthletes();
        void SaveTempData(string data);
        string GetTempData();
    }
}

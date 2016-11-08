using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceService
    {
        List<Triathlete> GetAthletes(IList<int> raceIds, IList<int> agegroupIds, IList<int> genderIds);

    }

}

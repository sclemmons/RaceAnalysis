﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.Service.Interfaces
{
    public interface IRaceCriteria
    {
        IList<string> SelectedRaceIds { get; set; }
        IList<int> SelectedAgeGroupIds { get; set; }
        IList<int> SelectedGenderIds { get; set; }
        string ToString();
    }

    
}

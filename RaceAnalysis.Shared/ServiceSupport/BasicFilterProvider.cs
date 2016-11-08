using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceAnalysis.ServiceSupport
{
    /// <summary>
    /// The basic filter provider simply evaluates filters the given athletes and returns the filtered athletes
    /// </summary>
    public class BasicFilterProvider
    {
        private IDurationFilter _Filter;
        private List<Triathlete> _Athletes;
        public BasicFilterProvider(List<Triathlete> athletes, IDurationFilter filter)
        {
            _Filter = filter;
            _Athletes = athletes;
        }
        public List<Triathlete> GetAthletes()
        {
            var filteredList = new List<Triathlete>();
            foreach (var a in _Athletes)
            {
                if (EvaluateSwim(a)
                    && EvaluateBike(a)
                    && EvaluateRun(a)
                    && EvaluateFinish(a) )

                filteredList.Add(a);
            }
            return filteredList;
        }

        private bool EvaluateSwim(Triathlete athlete)
        {
            if (athlete.Swim >= _Filter.SwimLow
            &&
                athlete.Swim <= _Filter.SwimHigh)
            {
                return true;
            }
            return false;
        }
        private bool EvaluateBike(Triathlete athlete)
        {
            if (athlete.Bike >= _Filter.BikeLow
            &&
                athlete.Bike <= _Filter.BikeHigh)
            {
                return true;
            }
            return false;
        }
        private bool EvaluateRun(Triathlete athlete)
        {
            if (athlete.Run >= _Filter.RunLow
            &&
                athlete.Run <= _Filter.RunHigh)
            {
                return true;
            }
            return false;
        }
        private bool EvaluateFinish(Triathlete athlete)
        {
            if (athlete.Finish >= _Filter.FinishLow
            &&
                athlete.Finish <= _Filter.FinishHigh)
            {
                return true;
            }
            return false;
        }

    }
}

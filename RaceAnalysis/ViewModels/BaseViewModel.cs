using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RaceAnalysis.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            Filter = new RaceFilterViewModel();
            Triathletes = new List<Triathlete>();

        }
        public Dictionary<string, BaseViewModel> Container { get; set; } //container for other ViewModels 
        public RaceFilterViewModel Filter { get; set; }
        public IEnumerable<Triathlete> Triathletes { get; set; }
    }
}
using RaceAnalysis.Service;
using RaceAnalysis.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis
{
    public class CacheConfig
    {
        private IRaceService _RaceService;
        public CacheConfig(IRaceService races)
        {
            _RaceService = races;
        }
        public static void Register()
        {
  
        }


    }
}
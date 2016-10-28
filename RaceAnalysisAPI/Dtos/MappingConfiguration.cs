using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using RaceAnalysis.Models;

namespace RaceAnalysisAPI.Dtos
{

    public static class MappingConfiguration
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Race, RaceDto>()
                .ForMember(d => d.NewDisplayName, opt => opt.MapFrom(s => s.DisplayName));

                      
        }
    }
}
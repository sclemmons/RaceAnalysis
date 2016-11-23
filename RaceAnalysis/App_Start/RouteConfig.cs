using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RaceAnalysis
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(); //map using attributes

            routes.MapRoute(
                name: "Summary",
                url: "summary/{action}/{id}",
                defaults: new { controller = "TriStatsSummary", action = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Details",
              url: "details/{action}/{id}",
              defaults: new { controller = "TriStats", action = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BikeRun",
                url: "bikerun/{action}/{id}",
                defaults: new { controller = "CompareBikeRun", action = UrlParameter.Optional, id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Athletes",
               url: "athletes/{action}/{id}",
               defaults: new { controller = "TriAthletes", action = UrlParameter.Optional, id = UrlParameter.Optional }
               );
            routes.MapRoute(
              name: "AgeGroups",
              url: "agegroupcompare/{action}/{id}",
              defaults: new { controller = "TriStatsAgeGroupsCompare", action = UrlParameter.Optional, id = UrlParameter.Optional }
              );

            routes.MapRoute(
            name: "Races",
            url: "racecompare/{action}/{id}",
            defaults: new { controller = "CompareRaces", action = UrlParameter.Optional, id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}

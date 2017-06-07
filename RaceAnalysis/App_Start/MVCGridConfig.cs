using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCGrid;
using MVCGrid.Web;
using MVCGrid.Models;
using RaceAnalysis.Models;


namespace RaceAnalysis
{
    public class MVCGridConfig
    {
        private static int pageMax = 100;
        private static int itemsPerPage = 10;

        public static void RegisterGrids()
        {
            RegisterRaceAggregateGrid();
            RegisterAgeGroupAggregateGrid();
            RegisterRaceAdminGrid();
        }//RegisterGrids

        private static void RegisterRaceAggregateGrid()
        {
            MVCGridDefinitionTable.Add("RaceAggregateGrid", new MVCGridBuilder<RaceAggregate>()
              .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              .AddColumns(cols =>
              {
                  cols.Add("RaceName").WithSorting(true).WithValueExpression(p => p.Race.LongDisplayName);

				  cols.Add("Segment").WithSorting(true).WithValueExpression(p => p.Segment);

				  cols.Add("Count").WithSorting(true).WithValueExpression(p => p.AthleteCount.ToString());

				  cols.Add("SwimMedian").WithSorting(true).WithHeaderText("Swim Median")
                      .WithValueExpression(p => p.SwimMedian.ToString());
                  cols.Add("BikeMedian").WithSorting(true).WithHeaderText("Bike Median")
                      .WithValueExpression(p => p.BikeMedian.ToString());
                  cols.Add("RunMedian").WithSorting(true).WithHeaderText("Run Median")
                                        .WithValueExpression(p => p.RunMedian.ToString());
                  cols.Add("FinishMedian").WithSorting(true).WithHeaderText("Finish Median")
                                        .WithValueExpression(p => p.FinishMedian.ToString());


                  cols.Add("SwimFastest").WithSorting(true).WithHeaderText("Swim Fastest")
                      .WithValueExpression(p => p.SwimFastest.ToString());

                  cols.Add("BikeFastest").WithSorting(true).WithHeaderText("Bike Fastest")
                      .WithValueExpression(p => p.BikeFastest.ToString());
                  cols.Add("RunFastest").WithSorting(true).WithHeaderText("Run Fastest")
                                        .WithValueExpression(p => p.RunFastest.ToString());
                  cols.Add("FinishFastest").WithSorting(true).WithHeaderText("Finish Fastest")
                                        .WithValueExpression(p => p.FinishFastest.ToString());

                  cols.Add("DNFCount").WithSorting(true).WithHeaderText("DNF Count")
                                        .WithValueExpression(p => p.DNFCount.ToString());
                  cols.Add("Distance").WithVisibility(false)
                     .WithFiltering(true)
                     .WithHeaderText("Distance")
                    .WithValueExpression(p => p.Race.Distance);
              })
              .WithAdditionalQueryOptionNames("Search")
              .WithAdditionalSetting("RenderLoadingDiv", false)
              .WithFiltering(true)
              .WithSorting(true, "FinishMedian", SortDirection.Asc)
              .WithPaging(true, itemsPerPage, true, pageMax)
              .WithRetrieveDataMethod((context) =>
              {
                  var options = context.QueryOptions;
                  string distance = String.IsNullOrEmpty(options.GetFilterString("Distance")) ? "140.6" : options.GetFilterString("Distance");
                  string globalSearch = options.GetAdditionalQueryOptionString("Search");
                  var result = new QueryResult<RaceAggregate>();
                  using (var db = new RaceAnalysisDbContext())
                  {
                      var query = db.RacesAggregates.Include("Race").Where(p => p.Race.Distance == distance);
                      if (!String.IsNullOrEmpty(globalSearch))
                          query = query.Where(p => p.Race.LongDisplayName.Contains(globalSearch));
                      result.TotalRecords = query.Count();

                      if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                      {
                          switch (options.SortColumnName.ToLower())
                          {
                              case "racename":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.Race.LongDisplayName)
                                                : query.OrderByDescending(p => p.Race.LongDisplayName);
                                  break;

							  case "count":
								  query = options.SortDirection == SortDirection.Asc
												? query.OrderBy(p => p.AthleteCount)
												: query.OrderByDescending(p => p.AthleteCount);
								  break;

							  case "segment":
								  query = options.SortDirection == SortDirection.Asc
												? query.OrderBy(p => p.Segment)
												: query.OrderByDescending(p => p.Segment);
								  break;


							  case "swimfastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.SwimFastest)
                                           : query.OrderByDescending(p => p.SwimFastest);
                                  break;
                              case "bikefastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.BikeFastest)
                                           : query.OrderByDescending(p => p.BikeFastest);
                                  break;
                              case "runfastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.RunFastest)
                                           : query.OrderByDescending(p => p.RunFastest);
                                  break;
                              case "finishfastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.FinishFastest)
                                          : query.OrderByDescending(p => p.FinishFastest);
                                  break;

                              case "swimmedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.SwimMedian)
                                           : query.OrderByDescending(p => p.SwimMedian);
                                  break;
                              case "bikemedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.BikeMedian)
                                           : query.OrderByDescending(p => p.BikeMedian);
                                  break;
                              case "runmedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.RunMedian)
                                           : query.OrderByDescending(p => p.RunMedian);
                                  break;
                              case "finishmedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.FinishMedian)
                                          : query.OrderByDescending(p => p.FinishMedian);
                                  break;

                              case "dnfcount":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.DNFCount)
                                          : query.OrderByDescending(p => p.DNFCount);
                                  break;

                          }
                      }
                      if (options.GetLimitOffset().HasValue)
                      {
                          query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                      }
                      result.Items = query.ToList();
                  }

                  return result;
              })
          );
        }


        private static void RegisterAgeGroupAggregateGrid()
        {
            MVCGridDefinitionTable.Add("AgeGroupAggregateGrid", new MVCGridBuilder<AgeGroupAggregate>()
              .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              .AddColumns(cols =>
              {
                  cols.Add("RaceName").WithSorting(true).WithValueExpression(p => p.Race.LongDisplayName);
                  cols.Add("AgeGroup").WithSorting(true).WithValueExpression(p => p.AgeGroup.DisplayName);
                  cols.Add("Gender").WithSorting(true).WithValueExpression(p => p.Gender.DisplayName);

                  cols.Add("SwimMedian").WithSorting(true).WithHeaderText("Swim Median")
                      .WithValueExpression(p => p.SwimMedian.ToString());
                  cols.Add("BikeMedian").WithSorting(true).WithHeaderText("Bike Median")
                      .WithValueExpression(p => p.BikeMedian.ToString());
                  cols.Add("RunMedian").WithSorting(true).WithHeaderText("Run Median")
                                        .WithValueExpression(p => p.RunMedian.ToString());
                  cols.Add("FinishMedian").WithSorting(true).WithHeaderText("Finish Median")
                                        .WithValueExpression(p => p.FinishMedian.ToString());


                  cols.Add("SwimFastest").WithSorting(true).WithHeaderText("Swim Fastest")
                      .WithValueExpression(p => p.SwimFastest.ToString());

                  cols.Add("BikeFastest").WithSorting(true).WithHeaderText("Bike Fastest")
                      .WithValueExpression(p => p.BikeFastest.ToString());
                  cols.Add("RunFastest").WithSorting(true).WithHeaderText("Run Fastest")
                                        .WithValueExpression(p => p.RunFastest.ToString());
                  cols.Add("FinishFastest").WithSorting(true).WithHeaderText("Finish Fastest")
                                        .WithValueExpression(p => p.FinishFastest.ToString());

                  cols.Add("DNFCount").WithSorting(true).WithHeaderText("DNF Count")
                                        .WithValueExpression(p => p.DNFCount.ToString());
                  cols.Add("Distance").WithVisibility(false)
                     .WithFiltering(true)
                     .WithHeaderText("Distance")
                    .WithValueExpression(p => p.Race.Distance);
              })
              .WithAdditionalQueryOptionNames("Search")
              .WithAdditionalSetting("RenderLoadingDiv", false)
              .WithFiltering(true)
              .WithSorting(true, "RaceName", SortDirection.Asc)
              .WithPaging(true, itemsPerPage, true, pageMax)
              .WithRetrieveDataMethod((context) =>
              {
                  var options = context.QueryOptions;
                  string distance = String.IsNullOrEmpty(options.GetFilterString("Distance")) ? "140.6" : options.GetFilterString("Distance");
                  string globalSearch = options.GetAdditionalQueryOptionString("Search");
                  var result = new QueryResult<AgeGroupAggregate>();
                  using (var db = new RaceAnalysisDbContext())
                  {
                      var query = db.AgeGroupAggregates.Include("Race").Include("AgeGroup").Include("Gender")
                                                        .Where(p => p.Race.Distance == distance);

                      if (!String.IsNullOrEmpty(globalSearch))
                          query = query.Where(p => p.Race.LongDisplayName.Contains(globalSearch));
                      result.TotalRecords = query.Count();

                      if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                      {
                          switch (options.SortColumnName.ToLower())
                          {
                              case "racename":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.Race.LongDisplayName).ThenBy(p => p.AgeGroup.DisplayOrder)
                                                : query.OrderByDescending(p => p.Race.LongDisplayName).ThenBy(p => p.AgeGroup.DisplayOrder);
                                  break;

                              case "agegroup":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.AgeGroup.DisplayName)
                                                : query.OrderByDescending(p => p.Race.DisplayName);
                                  break;
                              case "gender":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.Gender.DisplayName)
                                                : query.OrderByDescending(p => p.Gender.DisplayName);
                                  break;

                              case "swimfastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.SwimFastest)
                                           : query.OrderByDescending(p => p.SwimFastest);
                                  break;
                              case "bikefastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.BikeFastest)
                                           : query.OrderByDescending(p => p.BikeFastest);
                                  break;
                              case "runfastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.RunFastest)
                                           : query.OrderByDescending(p => p.RunFastest);
                                  break;
                              case "finishfastest":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.FinishFastest)
                                          : query.OrderByDescending(p => p.FinishFastest);
                                  break;

                              case "swimmedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.SwimMedian)
                                           : query.OrderByDescending(p => p.SwimMedian);
                                  break;
                              case "bikemedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.BikeMedian)
                                           : query.OrderByDescending(p => p.BikeMedian);
                                  break;
                              case "runmedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.RunMedian)
                                           : query.OrderByDescending(p => p.RunMedian);
                                  break;
                              case "finishmedian":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.FinishMedian)
                                          : query.OrderByDescending(p => p.FinishMedian);
                                  break;

                              case "dnfcount":
                                  query = options.SortDirection == SortDirection.Asc
                                           ? query.OrderBy(p => p.DNFCount)
                                          : query.OrderByDescending(p => p.DNFCount);
                                  break;

                          }
                      }
                      if (options.GetLimitOffset().HasValue)
                      {
                          query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                      }
                      result.Items = query.ToList();
                  }

                  return result;
              })
          );
        }


        private static void RegisterRaceAdminGrid()
        {
            MVCGridDefinitionTable.Add("RaceAdminGrid", new MVCGridBuilder<Race>()
              .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              .AddColumns(cols =>
              {
                  cols.Add("RaceName").WithSorting(true).WithValueExpression(p => p.LongDisplayName);
                  cols.Add("RaceId").WithSorting(true).WithValueExpression(p => p.RaceId);
                  cols.Add("RaceDate").WithSorting(true).WithValueExpression(p => p.RaceDate.ToShortDateString());
                  cols.Add("Edit").WithHtmlEncoding(false)
                    .WithSorting(false)
                    .WithHeaderText(" ")
                    .WithValueExpression((p, c) => c.UrlHelper.Action("Edit", "Races", new { id = p.RaceId }))
                    .WithValueTemplate("<a href='{Value}' class='btn btn-primary' role='button'>Edit</a>");

                  cols.Add("Populate").WithHtmlEncoding(false)
                      .WithSorting(false)
                      .WithHeaderText(" ")
                      .WithValueExpression((p, c) => c.UrlHelper.Action("CacheFill", "Races", new { id = p.RaceId }))
                      .WithValueTemplate("<a href='{Value}' class='btn btn-primary' role='button'>Populate</a>");

                  
                  cols.Add("ValidateMessage").WithHtmlEncoding(false).WithSorting(true)
                     .WithValueExpression((p, c) => c.UrlHelper.Action("Validate", "Races", new { id = p.RaceId}))
                     .WithValueTemplate(              
                     "<a href='{Value}' class='btn {Model.ValidateMessage}' role='button'>{Model.ValidateMessage}</a>");

                  cols.Add("IsAggregated").WithHtmlEncoding(false).WithSorting(true)
                      .WithValueExpression((p, c) => c.UrlHelper.Action("Aggregate", "Races", new { id = p.RaceId }))
                      .WithValueTemplate(
                      "<a href='{Value}' class='btn {Model.IsAggregated}' role='button'>{Model.IsAggregated}</a>");

                  cols.Add("Distance").WithVisibility(false)
                         .WithFiltering(true)
                         .WithHeaderText("Distance")
                        .WithValueExpression(p => p.Distance);
              })
              .WithAdditionalQueryOptionNames("Search")
              .WithAdditionalSetting("RenderLoadingDiv", false)
              .WithFiltering(true)
              .WithSorting(true, "RaceName", SortDirection.Asc)
              .WithPaging(true, itemsPerPage, true, pageMax)
              .WithRetrieveDataMethod((context) =>
              {
                  var options = context.QueryOptions;
                  string distance = String.IsNullOrEmpty(options.GetFilterString("Distance")) ? "140.6" : options.GetFilterString("Distance");
                  string globalSearch = options.GetAdditionalQueryOptionString("Search");
                  var result = new QueryResult<Race>();
                  using (var db = new RaceAnalysisDbContext())
                  {
                      var query = db.Races.Where(p => p.Distance == distance);

                      if (!String.IsNullOrEmpty(globalSearch))
                          query = query.Where(p => p.LongDisplayName.Contains(globalSearch));
                      result.TotalRecords = query.Count();

                      if (!String.IsNullOrWhiteSpace(options.SortColumnName))
                      {
                          switch (options.SortColumnName.ToLower())
                          {
                              case "racename":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.LongDisplayName)
                                                : query.OrderByDescending(p => p.LongDisplayName);
                                  break;
                              case "raceid":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.RaceId)
                                                : query.OrderByDescending(p => p.RaceId);
                                  break;
                              case "racedate":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.RaceDate)
                                                : query.OrderByDescending(p => p.RaceDate);
                                  break;

                              case "validatemessage":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.ValidateMessage)
                                                : query.OrderByDescending(p => p.ValidateMessage);
                                  break;

                              case "isaggregated":
                                  query = options.SortDirection == SortDirection.Asc
                                                ? query.OrderBy(p => p.IsAggregated)
                                                : query.OrderByDescending(p => p.IsAggregated);
                                  break;



                          }
                      }
                      if (options.GetLimitOffset().HasValue)
                      {
                          query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                      }
                      result.Items = query.ToList();
                  }

                  return result;
              })
          );
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{

    public class TriStatsViewModel :BaseViewModel
    {
        public TriStatsViewModel()
        {
            //to prevent nulls
            Stats = new List<TriStats>();
            UniqueId = Guid.NewGuid(); 
        }
      
        public List<TriStats> Stats { get; set; }
        public Guid UniqueId { get; set; }//this is overkill for our need, but easy for now.

        /// <summary>
        /// create data for a chart that compares each race
        /// </summary>
        public GoogleVisualizationDataTable RaceComparisonChartDataMedian
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();
                var orderedStats = Stats.OrderBy(s => s.Race.RaceDate);
                dataTable.AddColumn("Races", "string", "domain"); //our header column

                var races = new List<Race>();  //we are going to compare races, so build out the race list and our columns
                foreach (TriStats stat in orderedStats)
                {
                    races.Add(stat.Race);
                    dataTable.AddColumn(stat.Race.DisplayName, "timeofday", "data"); //define the data type the we will be populating in the rows
                }


                var swimRow = new List<object>(Stats.Count + 1); //add 1 for our header column
                dataTable.AddRow(swimRow);
                var bikeRow = new List<object>();
                dataTable.AddRow(bikeRow);
                var runRow = new List<object>();
                dataTable.AddRow(runRow);
                var finishRow = new List<object>();
                dataTable.AddRow(finishRow);

                //our header values: 
                swimRow.Add("swim");
                bikeRow.Add("bike");
                runRow.Add("run");
                finishRow.Add("finish");

                foreach (TriStats stat in orderedStats)
                {
//                    raceCol++;  //each stat and each column represents a different race in this chart

                    swimRow.Add( new List<object> { stat.Swim.Median.Hours, stat.Swim.Median.Minutes, stat.Swim.Median.Seconds } ); //each of these sub-arrays are to build the time for 1 thing we are measuring
                    bikeRow.Add( new int[] { stat.Bike.Median.Hours, stat.Bike.Median.Minutes, stat.Bike.Median.Seconds });
                    runRow.Add( new int[] { stat.Run.Median.Hours, stat.Run.Median.Minutes, stat.Run.Median.Seconds });
                    finishRow.Add( new int[] { stat.Finish.Median.Hours, stat.Finish.Median.Minutes, stat.Finish.Median.Seconds });

                }

                return dataTable;
            }

        }

        public GoogleVisualizationDataTable RaceComparisonTableDataMedian
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();
                var orderedStats = Stats.OrderBy(s => s.Race.RaceDate);

                dataTable.AddColumn("Races", "string", "domain"); //our header column
                dataTable.AddColumn("Swim", "timeofday", "data"); 
                dataTable.AddColumn("Bike", "timeofday", "data"); 
                dataTable.AddColumn("Run", "timeofday", "data"); 
                dataTable.AddColumn("Finish", "timeofday", "data"); 


                foreach (TriStats stat in orderedStats)
                {
                    var row = new List<object>();

                    row.Add(stat.Race.DisplayName);
                    row.Add(new int[] { stat.Swim.Median.Hours, stat.Swim.Median.Minutes, stat.Swim.Median.Seconds }); //each of these sub-arrays are to build the time for 1 thing we are measuring
                    row.Add(new int[] { stat.Bike.Median.Hours, stat.Bike.Median.Minutes, stat.Bike.Median.Seconds });
                    row.Add(new int[] { stat.Run.Median.Hours, stat.Run.Median.Minutes, stat.Run.Median.Seconds });
                    row.Add(new int[] { stat.Finish.Median.Hours, stat.Finish.Median.Minutes, stat.Finish.Median.Seconds });

                    dataTable.AddRow(row);
                }

                return dataTable;
            }

        }



        public GoogleVisualizationDataTable RaceComparisonTableDataFastest
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();
                var orderedStats = Stats.OrderBy(s => s.Race.RaceDate);

                dataTable.AddColumn("Races", "string", "domain"); //our header column
                dataTable.AddColumn("Swim", "timeofday", "data");
                dataTable.AddColumn("Bike", "timeofday", "data");
                dataTable.AddColumn("Run", "timeofday", "data");
                dataTable.AddColumn("Finish", "timeofday", "data");


                foreach (TriStats stat in orderedStats)
                {
                    var row = new List<object>();

                    row.Add(stat.Race.DisplayName);
                    row.Add(new int[] { stat.Swim.Min.Hours, stat.Swim.Min.Minutes, stat.Swim.Min.Seconds }); //each of these sub-arrays are to build the time for 1 thing we are measuring
                    row.Add(new int[] { stat.Bike.Min.Hours, stat.Bike.Min.Minutes, stat.Bike.Min.Seconds });
                    row.Add(new int[] { stat.Run.Min.Hours, stat.Run.Min.Minutes, stat.Run.Min.Seconds });
                    row.Add(new int[] { stat.Finish.Min.Hours, stat.Finish.Min.Minutes, stat.Finish.Min.Seconds });

                    dataTable.AddRow(row);
                }

                return dataTable;
            }

        }

        public GoogleVisualizationDataTable RaceComparisonChartDataFastest
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();
                var orderedStats = Stats.OrderBy(s => s.Race.RaceDate);
                dataTable.AddColumn("Races", "string", "domain"); //our header column

                var races = new List<Race>();  //we are going to compare races, so build out the race list and our columns
                foreach (TriStats stat in orderedStats)
                {
                    races.Add(stat.Race);
                    dataTable.AddColumn(stat.Race.DisplayName, "timeofday", "data"); //define the data type the we will be populating in the rows
                }


                var swimRow = new List<object>(Stats.Count + 1); //add 1 for our header column
                dataTable.AddRow(swimRow);
                var bikeRow = new List<object>();
                dataTable.AddRow(bikeRow);
                var runRow = new List<object>();
                dataTable.AddRow(runRow);
                var finishRow = new List<object>();
                dataTable.AddRow(finishRow);

                //our header values: 
                swimRow.Add("swim");
                bikeRow.Add("bike");
                runRow.Add("run");
                finishRow.Add("finish");

                
                //assign values to each column. 
                foreach (TriStats stat in orderedStats)
                {
                   //each stat and each column represents a different race in this chart

                    swimRow.Add(new List<object> { stat.Swim.Min.Hours, stat.Swim.Min.Minutes, stat.Swim.Min.Seconds }); //each of these sub-arrays are to build the time for 1 thing we are measuring
                    bikeRow.Add(new int[] { stat.Bike.Min.Hours, stat.Bike.Min.Minutes, stat.Bike.Min.Seconds });
                    runRow.Add(new int[] { stat.Run.Min.Hours, stat.Run.Min.Minutes, stat.Run.Min.Seconds });
                    finishRow.Add(new int[] { stat.Finish.Min.Hours, stat.Finish.Min.Minutes, stat.Finish.Min.Seconds });

                }

                return dataTable;
            }

        }

        public List<object> RaceComparisonChartDNF
        {
            get
            {
                var orderedStats = Stats.OrderBy(s => s.Race.RaceDate);
                var list = new List<object>();
                list.Add(new object[] { "Race","# DNFs" });

                foreach (TriStats stat in orderedStats)
                {
                    //each stat and each column represents a different race in this chart
                    list.Add(new object[] {
                                stat.Race.DisplayName,
                                stat.DNFCount
                    });
                }
               

                return list;
            }

        }


    }
}

    
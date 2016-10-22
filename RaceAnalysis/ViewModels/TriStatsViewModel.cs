﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{

    public class TriStatsViewModel
    {
        public TriStatsViewModel()
        {
            //to prevent nulls
            Filter = new RaceFilterViewModel();
            Stats = new List<TriStats>();
            UniqueId = Guid.NewGuid(); 
        }
        public RaceFilterViewModel Filter { get; set; }
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

                dataTable.AddColumn("Races", "string", "domain"); //our header column

                var races = new List<Race>();  //we are going to compare races, so build out the race list and our columns
                foreach (TriStats stat in Stats)
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
                foreach (TriStats stat in Stats)
                {
//                    raceCol++;  //each column represents a different race in this case

                    swimRow.Add( new List<object> { stat.Swim.Median.Hours, stat.Swim.Median.Minutes, stat.Swim.Median.Seconds } ); //each of these sub-arrays are to build the time for 1 thing we are measuring
                    bikeRow.Add( new int[] { stat.Bike.Median.Hours, stat.Bike.Median.Minutes, stat.Bike.Median.Seconds });
                    runRow.Add( new int[] { stat.Run.Median.Hours, stat.Run.Median.Minutes, stat.Run.Median.Seconds });
                    finishRow.Add( new int[] { stat.Finish.Median.Hours, stat.Finish.Median.Minutes, stat.Finish.Median.Seconds });

                }

                return dataTable;
            }

        }

    }
}

    
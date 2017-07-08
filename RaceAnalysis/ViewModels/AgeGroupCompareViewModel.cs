using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class AgeGroupCompareViewModel : TriStatsViewModel
    {
       
        public AgeGroupCompareViewModel()
        {
            
        }
     
        public GoogleVisualizationDataTable ChartDataMedian
        {
            get
            {                             
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Age Group", "string"); //our header column
                dataTable.AddColumn("Duration", "timeofday"); //our header column
   
                foreach (TriStats stat in Stats)//each stat is for a different age group
                {
                    var ag = Filter.AvailableAgeGroups.First(a => a.AgeGroupId == stat.AgeGroupId);
                    if (stat.Finish.Median.TotalSeconds > 0)
                    {
                        var row = new List<object>();
                        row.Add(ag.DisplayName);
                        row.Add(new int[] { stat.Finish.Median.Hours, stat.Finish.Median.Minutes, stat.Finish.Median.Seconds });
                        dataTable.AddRow(row);
                    }
                }
                return dataTable;
            }

        }

        public GoogleVisualizationDataTable ChartDataFastest
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Age Group", "string"); //our header column
                dataTable.AddColumn("Duration", "timeofday"); //our header column

                foreach (TriStats stat in Stats)
                {
                    var ag = Filter.AvailableAgeGroups.First(a => a.AgeGroupId == stat.AgeGroupId);
                    if (stat.Finish.Min.TotalSeconds > 0)
                    {
                        var row = new List<object>();
                        row.Add(ag.DisplayName);
                        row.Add(new int[] { stat.Finish.Min.Hours, stat.Finish.Min.Minutes, stat.Finish.Min.Seconds });
                        dataTable.AddRow(row);
                    }
                }

                return dataTable;
            }

        }
            

        public List<object> FinishersPerAgeGroup
        { 
            get
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var list = new List<object>();
                list.Add(new object[] { "Age Group", "# Count" });

               
                foreach (var id in AgeGroup.Expand(Filter.SelectedAgeGroupIds))
                {
                    var ag = Filter.AvailableAgeGroups.First(a => a.AgeGroupId == id);
                    list.Add(new object[] { ag.DisplayName, 0 });
               
                }

                var listEnum = list.GetEnumerator();
                listEnum.MoveNext();//the first row is our header
                listEnum.MoveNext(); //our first data row
                //assign values to each column. 
                foreach (TriStats stat in Stats)
                {

                    //each row represents a different AG that corresponds to the selected agegroups
                    var o = listEnum.Current as object[];
                    o[1] =  stat.Athletes.Count;
                    listEnum.MoveNext();
                }

                Trace.TraceInformation("FinishersPerAgeGroup: " + stopwatch.Elapsed);
                stopwatch.Stop();

                return list;
            }

        }

    }
}
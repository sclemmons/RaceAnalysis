using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
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

                dataTable.AddColumn("AgeGroups", "string", "domain"); //our header column

                var ageGroups = new List<AgeGroup>();  //we are going to compare aggroups, so build out the  list and our columns
                foreach (var id in AgeGroup.Expand(Filter.SelectedAgeGroupIds ))
                {
                    var ag = Filter.AvailableAgeGroups.First(a => a.AgeGroupId == id);
                    ageGroups.Add(ag);
                    dataTable.AddColumn(ag.DisplayName, "timeofday", "data"); //define the data type the we will be populating in the rows
            
                }
                             
                var finishRow = new List<object>();
                dataTable.AddRow(finishRow);

                //our header values: 
                finishRow.Add("Age Groups");

                
                
                //assign values to each column. 
                foreach (TriStats stat in Stats)
                {
                     //each row represents a different AG 
                    finishRow.Add(new int[] { stat.Finish.Median.Hours, stat.Finish.Median.Minutes, stat.Finish.Median.Seconds });
                }

                return dataTable;
            }

        }

        public GoogleVisualizationDataTable ChartDataFastest
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("AgeGroups", "string", "domain"); //our header column

                var ageGroups = new List<AgeGroup>();  //we are going to compare aggroups, so build out the  list and our columns
                foreach (var id in AgeGroup.Expand(Filter.SelectedAgeGroupIds))
                {
                    var ag = Filter.AvailableAgeGroups.First(a => a.AgeGroupId == id);
                    ageGroups.Add(ag);
                    dataTable.AddColumn(ag.DisplayName, "timeofday", "data"); //define the data type the we will be populating in the rows

                }

                var finishRow = new List<object>();
                dataTable.AddRow(finishRow);

                //our header values: 
                finishRow.Add("Age Groups");



                //assign values to each column. 
                foreach (TriStats stat in Stats)
                {
                    //each row represents a different AG 
                    finishRow.Add(new int[] { stat.Finish.Min.Hours, stat.Finish.Min.Minutes, stat.Finish.Min.Seconds });
                }

                return dataTable;
            }

        }


        public List<object> FinishersPerAgeGroup
        { 
            get
            {
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


                return list;
            }

        }

    }
}
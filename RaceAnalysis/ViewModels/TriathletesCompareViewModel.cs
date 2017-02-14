using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class TriathletesCompareViewModel : BaseViewModel
    {

        public TriathletesCompareViewModel()
        {
          
        }
        public TriStatsExtended Stats { get; set; }

        public GoogleVisualizationDataTable ChartData
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string", "domain");
                dataTable.AddColumn("Swim", "timeofday", "data");
                dataTable.AddColumn("Bike", "timeofday", "data");
                dataTable.AddColumn("Run", "timeofday", "data");
                dataTable.AddColumn("T1 & T2", "timeofday", "data");
                dataTable.AddColumn("", "string", "annotation");


                //assign values to each column. 
                foreach (Triathlete athlete in Triathletes)
                {
                    var row = new List<object>();
                    row.Add(new object[] { athlete.Name + "(" +  athlete.Race.DisplayName + ")" });
                    row.Add(new object[]
                     { athlete.Swim.Hours, athlete.Swim.Minutes, athlete.Swim.Seconds });

                    row.Add(new object[]
                      { athlete.Bike.Hours, athlete.Bike.Minutes, athlete.Bike.Seconds });

                    row.Add(new object[]
                       { athlete.Run.Hours, athlete.Run.Minutes, athlete.Run.Seconds });

                    TimeSpan tTime = athlete.Finish - (athlete.Swim + athlete.Bike + athlete.Run);

                    row.Add(new object[]
                         { tTime.Hours, tTime.Minutes, tTime.Seconds });

                    row.Add(new object[] { "" });
                    dataTable.AddRow(row);
                }

                return dataTable;
            }

        }


    }
}
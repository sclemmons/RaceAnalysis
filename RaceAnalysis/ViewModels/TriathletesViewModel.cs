using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using System.Collections;

namespace RaceAnalysis.Models
{
    public class TriathletesViewModel :BaseViewModel
    {
        public int TotalCount { get; set; }

        public List<object> BikeAndRunData
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "--", "(Bike,Run)" }); //google charts appears to have a bug where it doesn't show the first column

                foreach (var t in Triathletes)
                {
                    list.Add(new object[] {
                             new int[] {t.Bike.Hours, t.Bike.Minutes, t.Bike.Seconds },
                             new int[] { t.Run.Hours, t.Run.Minutes, t.Run.Seconds } 
                               
                    });

                }

                return list;
            }

        }

        public GoogleVisualizationDataTable BikeAndRunDataDoesNotWorkRightToo
        {  //google is not formatting one of the time series correctly
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("yaya", "string", "domain");//header column must have
                dataTable.AddColumn("Bike", "timeofday", "data");
                dataTable.AddColumn("Run", "timeofday", "data");

                foreach (var t in Triathletes)
                {
                    var row = new List<object>();
                    row.Add(new int[] { t.Bike.Hours, t.Bike.Minutes, t.Bike.Seconds });
                    row.Add(new int[] { t.Run.Hours, t.Run.Minutes, t.Run.Seconds });

                    dataTable.AddRow(row);
                }

                return dataTable;
            }
        }
        public GoogleVisualizationDataTable BikeAndRunDataDoesNotWorkRight
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("yaya", "string", "domain");
                dataTable.AddColumn("(Bike,Run)", "timeofday", "data");
                dataTable.AddColumn("(Bike,Run)", "timeofday", "data");

                foreach (var t in Triathletes)
                {
                    var row = (new object[] {
                             new int[] {t.Bike.Hours, t.Bike.Minutes, t.Bike.Seconds },
                             new int[] { t.Run.Hours, t.Run.Minutes, t.Run.Seconds }

                    });

                    dataTable.AddRow(row);
                }

              

                return dataTable;
            }

        }

    }
}
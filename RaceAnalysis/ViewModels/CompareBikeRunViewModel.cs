using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class CompareBikeRunViewModel : TriathletesViewModel
    {

        public TimeSpan BikeMedian { get; set; }
        public TimeSpan RunMedian { get; set; }

        //just a convenient property so we can access it from view 
       
        public object[] MidPoint
        {
            get {
                return new object[] {
                    new int[] { BikeMedian.Hours,BikeMedian.Minutes,BikeMedian.Seconds},
                    new int[] {RunMedian.Hours,RunMedian.Minutes,RunMedian.Seconds}
                };
            }
        }

        public int[] BikeMid
        {
            get
            {
                return new int[]
                     { BikeMedian.Hours, BikeMedian.Minutes, BikeMedian.Seconds };
               
            }
        }

        public int[] RunMid
        {
            get
            {
                return new int[] { RunMedian.Hours, RunMedian.Minutes, RunMedian.Seconds };
                
            }
        }
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
                //add our midpoint so that we can find it in the view and highlight it
                list.Add(MidPoint);
                 
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
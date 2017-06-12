using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace RaceAnalysis.Models
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            Filter = new RaceFilterViewModel();
            Triathletes = new List<Triathlete>();

        }
        public Dictionary<string, BaseViewModel> InnerView { get; set; } //container for other ViewModels 
        public RaceFilterViewModel Filter { get; set; }
        public IEnumerable<Triathlete> Triathletes { get; set; }

        public TriStats RaceStats { get; set; }
        public TriStats RaceDivisionStats { get; set; }


        public GoogleVisualizationDataTable RaceMedianChart
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
          //      foreach (TriRaceStats RaceStats in TriRaceStatss)
                {
                    var row = new List<object>();

                    row.Add(new object[] { RaceStats.Race.DisplayName });

                    row.Add(new object[]
                     { RaceStats.Swim.Median.Hours, RaceStats.Swim.Median.Minutes, RaceStats.Swim.Median.Seconds });

                    row.Add(new object[]
                      { RaceStats.Bike.Median.Hours, RaceStats.Bike.Median.Minutes, RaceStats.Bike.Median.Seconds });

                    row.Add(new object[]
                       { RaceStats.Run.Median.Hours, RaceStats.Run.Median.Minutes, RaceStats.Run.Median.Seconds });
                    if (RaceStats.Finish.Median.TotalSeconds > 0)
                    {
                        TimeSpan tTime = RaceStats.Finish.Median - (RaceStats.Swim.Median + RaceStats.Bike.Median + RaceStats.Run.Median);
                        row.Add(new object[]
                         { tTime.Hours, tTime.Minutes, tTime.Seconds });
                    }
                    else
                    {
                        row.Add(new object[] { 0, 0, 0 });
                    }
                    row.Add(new object[] { "" });
                    dataTable.AddRow(row);
                }

                return dataTable;
            }

        }

    }
}
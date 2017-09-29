using RaceAnalysis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models 
{
    public class HomePageViewModel
    {
        public IEnumerable<Race> RecentRaces { get; set; }
        public IEnumerable<RaceAggregate> FastestIMRuns { get; set; }
        public IEnumerable<RaceAggregate> FastestIMBikes { get; set; }
        public IEnumerable<RaceAggregate> FastestIMSwims { get; set; }
        public IEnumerable<RaceAggregate> FastestIMFinishes { get; set; }

        public IEnumerable<Triathlete> FastestMaleFinishesIM { get; set; }
        public IEnumerable<Triathlete> FastestFemaleFinishesIM { get; set; }




        public GoogleVisualizationDataTable DataTableMostRecent
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race Date", "string");
                dataTable.AddColumn("Distance", "string");


                foreach (Race race in RecentRaces)
                {
                    var row = new List<object>();
                    row.Add(race.LongDisplayName);
                    row.Add(race.RaceDate.ToShortDateString());
                    row.Add(race.Distance);
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }
        

        public GoogleVisualizationDataTable DataTableFastestIMFinish
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race Date", "string");
                dataTable.AddColumn("Distance", "string");
                dataTable.AddColumn("Median Time", "timeofday");


                foreach (RaceAggregate ag in FastestIMFinishes)
                {
                    var row = new List<object>();
                    row.Add(ag.Race.LongDisplayName);
                    row.Add(ag.Race.RaceDate.ToShortDateString());
                    row.Add(ag.Race.Distance);
                    row.Add(new List<object> { ag.FinishMedian.Hours, ag.FinishMedian.Minutes, ag.FinishMedian.Seconds });
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }
        public GoogleVisualizationDataTable DataTableFastestIMSwim
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race Date", "string");
                dataTable.AddColumn("Distance", "string");
                dataTable.AddColumn("Median Time", "timeofday");


                foreach (RaceAggregate ag in FastestIMSwims)
                {
                    var row = new List<object>();
                    row.Add(ag.Race.LongDisplayName);
                    row.Add(ag.Race.RaceDate.ToShortDateString());
                    row.Add(ag.Race.Distance);
                    row.Add(new List<object> { ag.FinishMedian.Hours, ag.FinishMedian.Minutes, ag.FinishMedian.Seconds });
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }
        public GoogleVisualizationDataTable DataTableFastestIMBike
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race Date", "string");
                dataTable.AddColumn("Distance", "string");
                dataTable.AddColumn("Median Time", "timeofday");


                foreach (RaceAggregate ag in FastestIMBikes)
                {
                    var row = new List<object>();
                    row.Add(ag.Race.LongDisplayName);
                    row.Add(ag.Race.RaceDate.ToShortDateString());
                    row.Add(ag.Race.Distance);
                    row.Add(new List<object> { ag.FinishMedian.Hours, ag.FinishMedian.Minutes, ag.FinishMedian.Seconds });
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }
        public GoogleVisualizationDataTable DataTableFastestIMRun
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race Date", "string");
                dataTable.AddColumn("Distance", "string");
                dataTable.AddColumn("Median Time", "timeofday");


                foreach (RaceAggregate ag in FastestIMRuns)
                {
                    var row = new List<object>();
                    row.Add(ag.Race.LongDisplayName);
                    row.Add(ag.Race.RaceDate.ToShortDateString());
                    row.Add(ag.Race.Distance);
                    row.Add(new List<object> { ag.FinishMedian.Hours, ag.FinishMedian.Minutes, ag.FinishMedian.Seconds });
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }

        public GoogleVisualizationDataTable DataTableFastestIMMale
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race", "string");
                dataTable.AddColumn("Time", "timeofday");


                foreach (Triathlete t in FastestMaleFinishesIM)
                {
                    var row = new List<object>();
                    row.Add(t.Name);
                    row.Add(t.Race.DisplayName);
                    row.Add(new List<object> { t.Finish.Hours, t.Finish.Minutes, t.Finish.Seconds });
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }

        public GoogleVisualizationDataTable DataTableFastestIMFemale
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();

                dataTable.AddColumn("Name", "string");
                dataTable.AddColumn("Race", "string");
                dataTable.AddColumn("Time", "timeofday");


                foreach (Triathlete t in FastestFemaleFinishesIM)
                {
                    var row = new List<object>();
                    row.Add(t.Name);
                    row.Add(t.Race.DisplayName);
                    row.Add(new List<object> { t.Finish.Hours, t.Finish.Minutes, t.Finish.Seconds });
                    dataTable.AddRow(row);
                }
                return dataTable;
            }

        }


    }
}
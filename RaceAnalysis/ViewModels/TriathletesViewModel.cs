using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using X.PagedList;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace RaceAnalysis.Models
{
    public class TriathletesViewModel :BaseViewModel
    {
        public int TotalCount { get; set; }

        public int SelectedAthleteId { get; set; }
        public string SelectedAthleteName { get; set; }

       
        public IEnumerable<ShallowTriathlete> Athletes { get; set; }



        public GoogleVisualizationDataTable AthletesTable
        {
            get
            {
                var dataTable = new GoogleVisualizationDataTable();
               
                dataTable.AddColumn("Race", "string", "domain"); 
                dataTable.AddColumn("Athlete", "string", "data");
                dataTable.AddColumn("Country", "string", "data");
                dataTable.AddColumn("Div Rank", "string", "data");
                dataTable.AddColumn("Gender Rank", "string", "data");
                dataTable.AddColumn("Overall Rank", "string", "data");
                dataTable.AddColumn("Swim", "timeofday", "data");
                dataTable.AddColumn("Bike", "timeofday", "data");
                dataTable.AddColumn("Run", "timeofday", "data");
                dataTable.AddColumn("Finish", "timeofday", "data");

                
                foreach (Triathlete athlete in base.Triathletes)
                {
                    var row = new List<object>();

                    row.Add(athlete.Race.DisplayName);
                    row.Add(athlete.Name);
                    row.Add(athlete.Country);
                    row.Add(athlete.DivRank);
                    row.Add(athlete.GenderRank);
                    row.Add(athlete.OverallRank);

                    row.Add(new int[] { athlete.Swim.Hours, athlete.Swim.Minutes, athlete.Swim.Seconds }); //each of these sub-arrays are to build the time for 1 thing we are measuring
                    row.Add(new int[] { athlete.Bike.Hours, athlete.Bike.Minutes, athlete.Bike.Seconds });
                    row.Add(new int[] { athlete.Run.Hours, athlete.Run.Minutes, athlete.Run.Seconds });
                    row.Add(new int[] { athlete.Finish.Hours, athlete.Finish.Minutes, athlete.Finish.Seconds });

                    dataTable.AddRow(row);
                }

                return dataTable;
            }

        }


        public GoogleVisualizationDataTable ComparisonChart
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

                    row.Add(new object[] { athlete.Race.DisplayName  + Environment.NewLine + athlete.Name });

                    row.Add(new object[]
                     { athlete.Swim.Hours, athlete.Swim.Minutes, athlete.Swim.Seconds });

                    row.Add(new object[]
                      { athlete.Bike.Hours, athlete.Bike.Minutes, athlete.Bike.Seconds });

                    row.Add(new object[]
                       { athlete.Run.Hours, athlete.Run.Minutes, athlete.Run.Seconds });
                    if (athlete.Finish.TotalSeconds > 0)
                    {
                        TimeSpan tTime = athlete.Finish - (athlete.Swim + athlete.Bike + athlete.Run);
                        row.Add(new object[]
                         { tTime.Hours, tTime.Minutes, tTime.Seconds });
                    }
                    else
                    {
                        row.Add(new object[]{ 0,0,0});
                    }
                    row.Add(new object[] { "" });
                    dataTable.AddRow(row);
                }

                return dataTable;
            }

        }
    }
}
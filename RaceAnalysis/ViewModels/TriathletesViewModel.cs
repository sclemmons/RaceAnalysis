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
        public string SelectedAgeGroup { get; set; } //the age group of the selected athlete
        public string SelectedGender { get; set; } //the gender of the selected athlete
       
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

                
                foreach (Triathlete athlete in base.Triathletes.OrderBy(t => t.Race.RaceDate))
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

        public GoogleVisualizationDataTable SingleRaceChart
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


                //athlete stats:
                foreach (Triathlete athlete in Triathletes)
                {
                    var row = new List<object>();

                    row.Add(new object[] { athlete.Name });

                    row.Add(new object[]
                     { athlete.Swim.Hours, athlete.Swim.Minutes, athlete.Swim.Seconds });

                    row.Add(new object[]
                      { athlete.Bike.Hours, athlete.Bike.Minutes, athlete.Bike.Seconds });

                    row.Add(new object[]
                       { athlete.Run.Hours, athlete.Run.Minutes, athlete.Run.Seconds });
                    if (athlete.Finish.TotalSeconds > 0)
                    {
                        TimeSpan finish = athlete.Finish;
                        TimeSpan total = (athlete.Swim + athlete.Bike + athlete.Run);
                        TimeSpan tTime;
                        if (finish > total)
                            tTime = finish - total;
                        else
                            tTime = total - finish;


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

                //race stats: 
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
                        TimeSpan finish = RaceStats.Finish.Median; 
                        TimeSpan total  = (RaceStats.Swim.Median + RaceStats.Bike.Median + RaceStats.Run.Median);
                        TimeSpan tTime;
                        if (finish > total)
                            tTime = finish - total;
                        else
                            tTime = total - finish;

                        row.Add(new object[]
                         { tTime.Hours, tTime.Minutes, tTime.Seconds });

                        //swim+bike+run=14:03:42  finish med=13:39:02 
                    }
                    else
                    {
                        row.Add(new object[] { 0, 0, 0 });
                    }
                    row.Add(new object[] { "" });
                    dataTable.AddRow(row);
                }

                //division stats
                {
                    
                    var row = new List<object>();

                    row.Add(new object[] { SelectedAgeGroup + " " + SelectedGender });

                    row.Add(new object[]
                     { RaceDivisionStats.Swim.Median.Hours, RaceDivisionStats.Swim.Median.Minutes, RaceDivisionStats.Swim.Median.Seconds });

                    row.Add(new object[]
                      { RaceDivisionStats.Bike.Median.Hours, RaceDivisionStats.Bike.Median.Minutes, RaceDivisionStats.Bike.Median.Seconds });

                    row.Add(new object[]
                       { RaceDivisionStats.Run.Median.Hours, RaceDivisionStats.Run.Median.Minutes, RaceDivisionStats.Run.Median.Seconds });
                    if (RaceDivisionStats.Finish.Median.TotalSeconds > 0)
                    {
                        TimeSpan finish = RaceDivisionStats.Finish.Median;
                        TimeSpan total = (RaceDivisionStats.Swim.Median + RaceDivisionStats.Bike.Median + RaceDivisionStats.Run.Median);
                        TimeSpan tTime;
                        if (finish > total)
                            tTime = finish - total;
                        else
                            tTime = total - finish;
                        
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
                foreach (Triathlete athlete in Triathletes.OrderBy(t => t.Race.RaceDate))
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
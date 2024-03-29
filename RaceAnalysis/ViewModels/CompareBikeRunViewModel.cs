﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class CompareBikeRunViewModel : TriStatsViewModel
    {
        public CompareBikeRunViewModel()
        {
            FastBikeFastRun = new List<Triathlete>();
            SlowBikeFastRun = new List<Triathlete>();
            SlowBikeSlowRun = new List<Triathlete>();
            FastBikeSlowRun = new List<Triathlete>();
        }
        public List<Triathlete> FastBikeFastRun { get; set; }
        public List<Triathlete> SlowBikeFastRun { get; set; }
        public List<Triathlete> SlowBikeSlowRun { get; set; }
        public List<Triathlete> FastBikeSlowRun { get; set; }


        public TimeSpan BikeMedian { get; set; }
        public TimeSpan RunMedian { get; set; }

        //just a convenient property so we can access it from view 
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
             
                 
                return list;
            }

        }
       


        public List<object> Q1BikeFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item1;
                var q1Bikers = Stats[0].Bike.FastestHalf.Item1;
                var q2Bikers = Stats[0].Bike.FastestHalf.Item2;
                var q3Bikers = Stats[0].Bike.SlowestHalf.Item1;
                var q4Bikers = Stats[0].Bike.SlowestHalf.Item2;


                list.Add(new object[] { "Q1 Bikers", finishers.Where(t => q1Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Bikers", finishers.Where(t => q2Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Bikers", finishers.Where(t => q3Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Bikers", finishers.Where(t => q4Bikers.Contains(t)).Count() });
             
                return list;
            }

        }

        public List<object> Q1RunFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item1;
                
                var q1Runners = Stats[0].Run.FastestHalf.Item1;
                var q2Runners = Stats[0].Run.FastestHalf.Item2;
                var q3Runners = Stats[0].Run.SlowestHalf.Item1;
                var q4Runners = Stats[0].Run.SlowestHalf.Item2;

                list.Add(new object[] { "Q1 Runners", finishers.Where(t => q1Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Runners", finishers.Where(t => q2Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Runners", finishers.Where(t => q3Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Runners", finishers.Where(t => q4Runners.Contains(t)).Count() });

                return list;
            }

        }


        public List<object> Q2BikeFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item2;
                var q1Bikers = Stats[0].Bike.FastestHalf.Item1;
                var q2Bikers = Stats[0].Bike.FastestHalf.Item2;
                var q3Bikers = Stats[0].Bike.SlowestHalf.Item1;
                var q4Bikers = Stats[0].Bike.SlowestHalf.Item2;

                list.Add(new object[] { "Q1 Bikers", finishers.Where(t => q1Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Bikers", finishers.Where(t => q2Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Bikers", finishers.Where(t => q3Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Bikers", finishers.Where(t => q4Bikers.Contains(t)).Count() });

                return list;
            }

        }
        public List<object> Q2RunFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item2;
               
                var q1Runners = Stats[0].Run.FastestHalf.Item1;
                var q2Runners = Stats[0].Run.FastestHalf.Item2;
                var q3Runners = Stats[0].Run.SlowestHalf.Item1;
                var q4Runners = Stats[0].Run.SlowestHalf.Item2;

             
                list.Add(new object[] { "Q1 Runners", finishers.Where(t => q1Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Runners", finishers.Where(t => q2Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Runners", finishers.Where(t => q3Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Runners", finishers.Where(t => q4Runners.Contains(t)).Count() });

                return list;
            }

        }






        public List<object> Q3BikeFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.SlowestHalf.Item1;
                var q1Bikers = Stats[0].Bike.FastestHalf.Item1;
                var q2Bikers = Stats[0].Bike.FastestHalf.Item2;
                var q3Bikers = Stats[0].Bike.SlowestHalf.Item1;
                var q4Bikers = Stats[0].Bike.SlowestHalf.Item2;

            
                list.Add(new object[] { "Q1 Bikers", finishers.Where(t => q1Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Bikers", finishers.Where(t => q2Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Bikers", finishers.Where(t => q3Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Bikers", finishers.Where(t => q4Bikers.Contains(t)).Count() });
             
                return list;
            }

        }

        public List<object> Q3RunFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.SlowestHalf.Item1;
         
                var q1Runners = Stats[0].Run.FastestHalf.Item1;
                var q2Runners = Stats[0].Run.FastestHalf.Item2;
                var q3Runners = Stats[0].Run.SlowestHalf.Item1;
                var q4Runners = Stats[0].Run.SlowestHalf.Item2;

                list.Add(new object[] { "Q1 Runners", finishers.Where(t => q1Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Runners", finishers.Where(t => q2Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Runners", finishers.Where(t => q3Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Runners", finishers.Where(t => q4Runners.Contains(t)).Count() });

                return list;
            }

        }


        public List<object> Q4BikeFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.SlowestHalf.Item2;
                var q1Bikers = Stats[0].Bike.FastestHalf.Item1;
                var q2Bikers = Stats[0].Bike.FastestHalf.Item2;
                var q3Bikers = Stats[0].Bike.SlowestHalf.Item1;
                var q4Bikers = Stats[0].Bike.SlowestHalf.Item2;


                list.Add(new object[] { "Q1 Bikers", finishers.Where(t => q1Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Bikers", finishers.Where(t => q2Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Bikers", finishers.Where(t => q3Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Bikers", finishers.Where(t => q4Bikers.Contains(t)).Count() });
      
                return list;
            }

        }

        public List<object> Q4RunFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.SlowestHalf.Item2;
         
                var q1Runners = Stats[0].Run.FastestHalf.Item1;
                var q2Runners = Stats[0].Run.FastestHalf.Item2;
                var q3Runners = Stats[0].Run.SlowestHalf.Item1;
                var q4Runners = Stats[0].Run.SlowestHalf.Item2;

                list.Add(new object[] { "Q1 Runners", finishers.Where(t => q1Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Runners", finishers.Where(t => q2Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Runners", finishers.Where(t => q3Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Runners", finishers.Where(t => q4Runners.Contains(t)).Count() });

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

        public List<object> TopHalfBikeFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item1.Concat(
                                Stats[0].Finish.FastestHalf.Item2);
                    
                var q1Bikers = Stats[0].Bike.FastestHalf.Item1;
                var q2Bikers = Stats[0].Bike.FastestHalf.Item2;
                var q3Bikers = Stats[0].Bike.SlowestHalf.Item1;
                var q4Bikers = Stats[0].Bike.SlowestHalf.Item2;

                /*
                var test = q3Bikers.ToList();
                var test1 = finishers.Where(t => q3Bikers.Contains(t));
                */
                list.Add(new object[] { "Q1 Bikers", finishers.Where(t => q1Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Bikers", finishers.Where(t => q2Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Bikers", finishers.Where(t => q3Bikers.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Bikers", finishers.Where(t => q4Bikers.Contains(t)).Count() });

                return list;
            }

        }

        public List<object> TopHalfRunFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item1.Concat(
                                Stats[0].Finish.FastestHalf.Item2);

                var q1Runners = Stats[0].Run.FastestHalf.Item1;
                var q2Runners = Stats[0].Run.FastestHalf.Item2;
                var q3Runners = Stats[0].Run.SlowestHalf.Item1;
                var q4Runners = Stats[0].Run.SlowestHalf.Item2;

                /***
                var test = q4Runners.ToList();
                var test2 = finishers.Where(t => q4Runners.Contains(t)).ToList();
                var test3 = Stats[0].Run.SlowestHalf.Item1.ToList();
                var test4 = finishers.Where(t => q3Runners.Contains(t)).ToList();
                ****/

                list.Add(new object[] { "Q1 Runners", finishers.Where(t => q1Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Runners", finishers.Where(t => q2Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Runners", finishers.Where(t => q3Runners.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Runners", finishers.Where(t => q4Runners.Contains(t)).Count() });

                return list;
            }

        }
        public List<object> TopHalfSwimFinishers
        {
            get
            {
                var list = new List<object>();
                list.Add(new object[] { "Split", "# of Athletes" }); //google charts appears to have a bug where it doesn't show the first column


                var finishers = Stats[0].Finish.FastestHalf.Item1.Concat(
                                Stats[0].Finish.FastestHalf.Item2);

                var q1Swimmers = Stats[0].Swim.FastestHalf.Item1;
                var q2Swimmers = Stats[0].Swim.FastestHalf.Item2;
                var q3Swimmers = Stats[0].Swim.SlowestHalf.Item1;
                var q4Swimmers = Stats[0].Swim.SlowestHalf.Item2;

                /*
                var test = q3Swimmers.ToList();
                var test1 = finishers.Where(t => q3Swimmers.Contains(t));
                */
                list.Add(new object[] { "Q1 Swimmers", finishers.Where(t => q1Swimmers.Contains(t)).Count() });
                list.Add(new object[] { "Q2 Swimmers", finishers.Where(t => q2Swimmers.Contains(t)).Count() });
                list.Add(new object[] { "Q3 Swimmers", finishers.Where(t => q3Swimmers.Contains(t)).Count() });
                list.Add(new object[] { "Q4 Swimmers", finishers.Where(t => q4Swimmers.Contains(t)).Count() });

                return list;
            }

        }

    }
}
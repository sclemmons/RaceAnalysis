using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using System.Net.Http;
using System.Collections;

namespace RaceAnalysis
{
    public class Program
    {

        
        public static int Main(string[] args)
        {
            var parms = ParseCommandLine(args);
            if (parms.Item1 == false)
            {
                WriteHelpText();
                return 0;
            }
            Console.WriteLine("Press a key to get triathletes");
            Console.ReadKey();

            var request = new Request(parms.Item2, parms.Item3, parms.Item4,parms.Item5);
            var result = request.GetTriathletes().GetAwaiter().GetResult();


#if DEBUG
            Console.WriteLine("Done with result: " + result);
            Console.WriteLine("Press any key to end program.");
            Console.ReadKey(); //just to keep the window open
#endif
            
            return int.Parse(result);
        }

        private static Tuple<bool,int,int[],int[],string> ParseCommandLine(string[] args)
        {
            if (args == null || args.Length < 4)
            {
                return Tuple.Create(false, 0, (int[])null,(int[])null,"");
            }


            string race = "", agegroups = "", genders = "", restUrl = "";
            foreach (string s in args)
            {
                string trimmed = s.Trim();

                if (trimmed.StartsWith("-r"))
                {
                    race = trimmed.Substring(2);
                }
                else if (trimmed.StartsWith("-ag"))
                {
                    agegroups = trimmed.Substring(3);

                }
                else if (trimmed.StartsWith("-g"))
                {
                    genders = trimmed.Substring(2);
                }
                else if(trimmed.StartsWith("-url"))
                {
                    restUrl = trimmed.Substring(4);
                }

            }
            if(String.IsNullOrEmpty(race) || String.IsNullOrEmpty(agegroups) || String.IsNullOrEmpty(genders) || String.IsNullOrEmpty(restUrl))
            {
                return Tuple.Create(false, 0, (int[])null, (int[])null, "");
            }

            var tuple = Tuple.Create
            (
                    true,
                    int.Parse(race),
                    agegroups.Split(',').Select(int.Parse).ToArray(),
                    genders.Split(',').Select(int.Parse).ToArray(),
                    restUrl
            );

            return tuple;

        }

        private static void WriteHelpText()
        {
            Console.WriteLine("Usage: -r = raceId, -ag = Age Group Ids, -g = Gender Ids, RestUrl -url");
            Console.WriteLine("For example: >>raceanalysis.console.exe -r1 -ag12,13,14 -g15,16 -urlhttp://localhost....");
            Console.WriteLine("Will populate triathletes cache for raceId 1, Age Groups 12,13,15 , Genders 15, 16");
            Console.WriteLine("This app presumes you know the Ids of each.");
            Console.WriteLine("Press any key to exit program.");
#if DEBUG
            Console.ReadKey();
#endif
        }
    }


public class Request
{
        private int        _raceId;
        private int[]      _ageGroupIds;
        private int[]      _genderIds;
        private string     _restURL;

    public   Request(int race, int[] agegroups, int[] genders,string url)
    {
            _raceId = race;
            _ageGroupIds = agegroups;
            _genderIds = genders;
            _restURL = url;
    }

    public async Task<string> GetTriathletes()
    {
            var url = _restURL
                 .AppendPathSegments("api", "Triathletes")  //for now we are hard coding this until we have other endpoints
                 .SetQueryParams(new
                 {
                     count = 1,
                     raceId = _raceId,
                     ageGroupIds = String.Join(",",_ageGroupIds),  //we have to pass these values as a comma delimited string array for the API
                     genderIds = String.Join(",", _genderIds)
                 });         
                    



            var result = await url.GetStringAsync();

            return result;

    }
}

}

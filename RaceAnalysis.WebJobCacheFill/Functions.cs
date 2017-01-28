using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure;
using RaceAnalysis.SharedQueueMessages;
using Flurl;
using Flurl.Http;

namespace RaceAnalysis.WebJobCacheFill
{
    public class Functions
    {
        static readonly private string _restURL;

        static Functions()
        {
            _restURL = CloudConfigurationManager.GetSetting("RaceAnalysis.RestURL");
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage(
            [QueueTrigger("triathletepullrequest")] FetchTriathletesMessage msg , TextWriter log)
        {
            log.WriteLine("Processing triathlete pull request");
            log.WriteLine(
            " Race: " + msg.RaceId
            + " Age Groups: " + msg.AgegroupIds.ToString()
            + " Genders: " + msg.GenderIds.ToString()
            );
            var count = GetTriathletes(msg.RaceId, msg.AgegroupIds, msg.GenderIds);

            log.Write("Count: " + count.Result);
        }
              
        private static Task<string> GetTriathletes(string raceId,int[] ageGroupIds,int[] genderIds)
        {
            var url = _restURL
                 .AppendPathSegments("api", "Triathletes","TriathletesCount")  //for now we are hard coding this until we have other endpoints
                 .SetQueryParams(new
                 {
                     raceId = raceId,
                     ageGroupIds = String.Join(",", ageGroupIds),  //we have to pass these values as a comma delimited string array for the API
                     genderIds = String.Join(",", genderIds)
                 });




            var result = url.GetStringAsync();

            return result;

        }
    }
}

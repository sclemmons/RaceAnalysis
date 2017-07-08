using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using RaceAnalysis.SharedQueueMessages;
using RaceAnalysis.Service;
using RaceAnalysis.Models;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.AthleteCacheJob
{
    public class Functions
    {
        //    static readonly private string _restURL;

        static Functions()
        {
            //  _restURL = CloudConfigurationManager.GetSetting("RaceAnalysis.RestURL");

        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public  static void ProcessQueueMessage(
                 [QueueTrigger("athletecacherequest")] CacheRaceAthletesMessage msg, TextWriter log)
        {
            log.WriteLine("Processing Athletes Cache request for race" + msg.RaceId);
            PopulateCache(msg.RaceId);

            log.Write("Completed PopulateCache");
        }

        private static void PopulateCache(string raceId)
        {
            using (var db = new RaceAnalysisDbContext())
            {
                var raceService = new RaceService(db);
                var athletes = raceService.GetAthletes(
                      new BasicRaceCriteria
                      {
                          SelectedRaceIds = new string[] { raceId },
                          SelectedAgeGroupIds = AgeGroup.Expand(new int[] { 0 }),
                          SelectedGenderIds = Gender.Expand(new int[] { 0 })
                      }
                );
            }

        }
    }
}

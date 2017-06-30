using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using RaceAnalysis.SharedQueueMessages;
using RaceAnalysis.Service;
using RaceAnalysis.Models;

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
        public async static void ProcessQueueMessage(
                 [QueueTrigger("shallowathletecacherequest")] CacheShallowAthletesMessage msg, TextWriter log)
        {
            log.WriteLine("Processing Shallow Athletes Cache request for race" + msg.RaceId);
            await PopulateCache(msg.RaceId);

            log.Write("Completed PopulateCache");
        }

        private static async Task PopulateCache(string raceId)
        {
            IQueryable<ShallowTriathlete> shallowAthleteQuery;
            using (var db = new RaceAnalysisDbContext())
            {
                var athleteQuery = db.Triathletes.Include("RequestContext.RaceId").Where(t => t.RequestContext.RaceId == raceId);

                shallowAthleteQuery = athleteQuery.OrderBy(t => t.Name).
                    Select(t =>
                            new ShallowTriathlete()
                            {
                                Name = t.Name,
                                Id = t.TriathleteId,
                                RaceId = t.RequestContext.Race.RaceId
                            });

                await CacheService.Instance.PopulateCacheAsync(shallowAthleteQuery.ToList());
            }

        }
    }
}

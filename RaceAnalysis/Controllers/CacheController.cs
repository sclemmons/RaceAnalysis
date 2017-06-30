using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.SharedQueueMessages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CacheController : Controller
    {
        private ICacheService _Cache;
        private RaceAnalysisDbContext _DbContext;
        private CloudQueue _CacheRequestQueue;

       

        public CacheController(ICacheService cacheService, RaceAnalysisDbContext db)
        {
            _Cache = cacheService;
            _DbContext = db;
        }

        // GET: Cache
        public ActionResult Index()
        {
            var viewModel = new CacheViewModel();
            viewModel.ShallowAthleteCount = 0; // _Cache.GetShallowAthletes("DUMMY").Count;
            return View(viewModel);
        }

        public ActionResult FlushShallowAthletes()
        {
            var viewModel = new CacheViewModel();

            _Cache.FlushShallowAthletes();
            viewModel.ShallowAthleteCount = 0; //  _Cache.GetShallowAthletes("DUMMY").Count;
            return View("Index", viewModel);
        }

        public async Task<ActionResult> FillShallowAthletes()
        {
            InitializeStorage();

            var raceIds = _DbContext.Races.Select(r => r.RaceId);

            foreach (var raceId in raceIds)
            {
                await AddQueueMessage(raceId);
            }

            var viewModel = new CacheViewModel();
            viewModel.ShallowAthleteCount = 0;
           
            return View("Index", viewModel);
        }

        private void InitializeStorage()
        {
            // Open storage account using credentials from .cscfg file.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());

            // Get context object for working with queues, and 
            // set a default retry policy appropriate for a web user interface.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            //queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the queue.
            _CacheRequestQueue = queueClient.GetQueueReference("shallowathletecacherequest");
            _CacheRequestQueue.CreateIfNotExists();

        }

        private async Task AddQueueMessage(string raceId)
        {
          
            var msg = new CacheShallowAthletesMessage()
            {
                RaceId = raceId,
            };
            var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(msg));
            await _CacheRequestQueue.AddMessageAsync(queueMessage);
        }
    }
}
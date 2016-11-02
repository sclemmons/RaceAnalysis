using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RaceAnalysis.Models;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Threading.Tasks;
using RaceAnalysis.SharedQueueMessages;
using Newtonsoft.Json;

namespace RaceAnalysis.Controllers
{
    public class RacesController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        private CloudQueue cacheRequestQueue;

        public RacesController()
        {
            InitializeStorage();

        }

        private void InitializeStorage()
        {
            // Open storage account using credentials from .cscfg file.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());

            // Get context object for working with blobs, and 
            // set a default retry policy appropriate for a web user interface.
            //var blobClient = storageAccount.CreateCloudBlobClient();
            //blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the blob container.
            //imagesBlobContainer = blobClient.GetContainerReference("images");

            // Get context object for working with queues, and 
            // set a default retry policy appropriate for a web user interface.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            //queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the queue.
            cacheRequestQueue = queueClient.GetQueueReference("triathletepullrequest");
            cacheRequestQueue.CreateIfNotExists();
            
        }
        // GET: Races
        public ActionResult Index()
        {
            var races = db.Races.Include(r => r.Conditions);
            return View(races.ToList());
        }

        // GET: Races/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // GET: Races/Create
        public ActionResult Create()
        {
   //         ViewBag.ConditionsId = new SelectList(db.RaceConditions, "RaceConditionsId", "SwimGeneral");
            return View();
        }

        // POST: Races/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  public ActionResult Create([Bind(Include = "RaceId,BaseURL,DisplayName,RaceDate,ShortName,Distance,ConditionsId")] Race race)
        public ActionResult Create(
                [Bind(Prefix = "Race", Include = "BaseURL,DisplayName,RaceDate,ShortName,Distance")]Race race,
                [Bind(Prefix = "Conditions", Include = "SwimGeneral,BikeGeneral,RunGeneral")]RaceConditions conditions)

        {
            if (ModelState.IsValid)
            {
                db.RaceConditions.Add(conditions);
                db.SaveChanges();

                race.Conditions = conditions;

                db.Races.Add(race);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
               //NOTE: This is a temporary workaround. The View needs the model and should display the correct error
               //but this view needs both a Race model and a Conditions Model and I need to build that ViewModel
                ModelState.AddModelError("", "ERROR!!");
            }
           

            return View();
        }

        // GET: Races/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // POST: Races/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  public ActionResult Edit([Bind(Include = "RaceId,BaseURL,DisplayName,RaceDate,ShortName,Distance,ConditionsId,Conditions.SwimGeneral,Conditions.BikeGeneral,Conditions.RunGeneral")] Race race)
        public ActionResult Edit(
                [Bind(Prefix ="Race", Include = "RaceId,ConditionsId,BaseURL,DisplayName,RaceDate,ShortName,Distance")]Race race,
                [Bind(Prefix ="Conditions", Include = "RaceConditionsId,SwimGeneral,BikeGeneral,RunGeneral")]RaceConditions conditions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(race).State = EntityState.Modified;
                db.SaveChanges();

                db.Entry(conditions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(race);
        }

        // GET: Races/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // POST: Races/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Race race = db.Races.Find(id);
            db.Races.Remove(race);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CacheFill(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }

            await AddQueueMessages(id.Value);

            return RedirectToAction("Index");
        }

        private async Task AddQueueMessages(int raceId)
        {
            var raceIds = new int[]{ raceId }; //for futer growth?
            var agegroupIds = db.AgeGroups.Select(ag => ag.AgeGroupId).ToArray();
            var genderIds = db.Genders.Select(g => g.GenderId).ToArray();

            foreach (int id in raceIds)
            {
                foreach (int ageId in agegroupIds)
                {
                    foreach (int genderId in genderIds) //create a queue message for each AgeGroup, gender
                    {
                       var msg =  new FetchTriathletesMessage()
                        {
                           RaceId = id,
                           AgegroupIds = new int[] { ageId },
                           GenderIds = new int[] { genderId }
                        };
                        var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(msg));
                        await cacheRequestQueue.AddMessageAsync(queueMessage);
                    }
                }
            }
        }

 

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

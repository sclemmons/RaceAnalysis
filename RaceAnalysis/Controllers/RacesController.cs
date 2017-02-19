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
using RaceAnalysis.Service.Interfaces;
using System;
using Microsoft.AspNet.Identity;

namespace RaceAnalysis.Controllers
{
    public class RacesController : BaseController
    {
      
        private CloudQueue cacheRequestQueue;
        private IIdentityMessageService _emailService;

        public RacesController(IRaceService raceService, IIdentityMessageService emailService) : base(raceService) 
        {
            _emailService = emailService;
        }

        // GET: Races
        public ActionResult Index()
        {
            var viewModel = new RaceFilterViewModel();

            return View(viewModel);
        }

        public ActionResult Search()
        {
            var viewModel = new RaceSearchViewModel();
            viewModel.Tags = _DBContext.Tags.ToList();

            return View(viewModel);
        }

        public ActionResult SearchRaces(FormCollection form)
        {

            if (!String.IsNullOrEmpty(form["SelectedSwimTags"]))
            {
                return SearchBySwimCondition(form["SelectedSwimTags"]);
            }
            else if (!String.IsNullOrEmpty(form["SelectedBikeTags"]))
            {
                return SearchByBikeCondition(form["SelectedBikeTags"]);
            }
            else if (!String.IsNullOrEmpty(form["SelectedRunTags"]))
            {
                return SearchByRunCondition(form["SelectedRunTags"]);
            }
            return HttpNotFound();

        }
        public PartialViewResult ShowRaceRequest()
        {
        
            return PartialView("_RaceRequest", new RequestRaceForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> SubmitRaceRequest(RequestRaceForm model)
        {
            bool isMessageSent = true;

            if (ModelState.IsValid)
            {
                string bodyText = "<p>The following is a race request: from {2}: </p><p>Race:{0}</p><p>Url: {1}</p><p>Message: {3}</p>";
                string body = string.Format(bodyText, model.RaceName,model.URL, model.Email, model.Message);

                try
                {
                    await _emailService.SendAsync(
                        new IdentityMessage
                        {
                            Subject = "Race Request Message",
                            Destination = "scott_clemmons@hotmail.com",
                            Body = body
                        }
                     );

                    // await RaceAnalysis.Service.EmailService.SendContactForm(model);
                }
                catch (Exception ex)
                {
                    isMessageSent = false;
                }
            }
            else
            {
                isMessageSent = false;
            }
            return PartialView("_SubmitMessage", isMessageSent);
        }
    

    public PartialViewResult SearchBySwimCondition(string searchstring)
        {
            //var races = _RaceService.GetRacesBySwimCondition(searchstring);

            var tagIds = searchstring.Split(',').Select(int.Parse).ToList();
            var races = _RaceService.GetRacesByTagId(tagIds);

            return PartialView("_SearchResults", races);
        }
        public PartialViewResult SearchByBikeCondition(string searchstring)
        {
            // var races = _RaceService.GetRacesByBikeCondition(searchstring);
            var tagIds = searchstring.Split(',').Select(int.Parse).ToList();
            var races = _RaceService.GetRacesByTagId(tagIds);
            return PartialView("_SearchResults",races);
        }
        public PartialViewResult SearchByRunCondition(string searchstring)
        {
            // var races = _RaceService.GetRacesByRunCondition(searchstring);
            var tagIds = searchstring.Split(',').Select(int.Parse).ToList();
            var races = _RaceService.GetRacesByTagId(tagIds);
            return PartialView("_SearchResults",races);
        }

        // GET: Races/Conditions/5
        public ActionResult Conditions(string id)  //this is the race id
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = _DBContext.Races.Find(id);
            if (race == null)

            {
                return HttpNotFound();
            }
            var viewModel = new RaceConditionsViewModel();
            viewModel.Race = race;
            viewModel.Tags = _DBContext.Tags.ToList();
            return View(viewModel);
            
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            var races = _DBContext.Races.Include(r => r.Conditions);
            return View(races.ToList());
        }


        // GET: Races/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = _DBContext.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // GET: Races/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
   //         ViewBag.ConditionsId = new SelectList(_DBContext.RaceConditions, "RaceConditionsId", "SwimLayout");
            return View();
        }

        // POST: Races/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(
                [Bind(Prefix = "Race", Include = "BaseURL,DisplayName,RaceDate,ShortName,Distance")]Race race)
             

        {
            race.Conditions = new RaceConditions();

            if (ModelState.IsValid)
            {
                _DBContext.Races.Add(race);
                _DBContext.SaveChanges();
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = _DBContext.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }

            var viewModel = new RaceViewModel();
            viewModel.Race = race;
        
            return View(viewModel);
        }

        // POST: Races/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(
                [Bind(Prefix ="Race", Include = "RaceId,ConditionsId,BaseURL,DisplayName,RaceDate,ShortName,Distance")]Race race,
                [Bind(Prefix ="Conditions", Include = "RaceConditionsId,SwimLayout,BikeLayout,RunLayout")]RaceConditions conditions)
        {
            if (ModelState.IsValid)
            {
                _DBContext.Entry(race).State = EntityState.Modified;
                _DBContext.SaveChanges();

                _DBContext.Entry(conditions).State = EntityState.Modified;
                _DBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(race);
        }

        // GET: Races/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Race race = _DBContext.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            return View(race);
        }

        // POST: Races/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            Race race = _DBContext.Races.Find(id);
            _DBContext.Races.Remove(race);
            _DBContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CacheFill(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            InitializeStorage();
            Race race = _DBContext.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }

            await AddQueueMessages(id);

            return RedirectToAction("Index");
        }

        private async Task AddQueueMessages(string raceId)
        {
            var raceIds = new string[]{ raceId }; //for futer growth?
            var agegroupIds = _DBContext.AgeGroups.Select(ag => ag.AgeGroupId).ToArray();
            var genderIds = _DBContext.Genders.Select(g => g.GenderId).ToArray();

            foreach (string id in raceIds)
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
                _DBContext.Dispose();
            }
            base.Dispose(disposing);
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

        protected override ActionResult DisplayResultsView(RaceFilterViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}

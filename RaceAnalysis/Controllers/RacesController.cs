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
using System.Text;
using X.PagedList;
using System.Web.Routing;
using System.Data.Entity.Migrations;
using RaceAnalysis.ServiceSupport;

namespace RaceAnalysis.Controllers
{
    public class RacesController : BaseController
    {
      
        private CloudQueue cacheRequestQueue;
        private IIdentityMessageService _emailService;
#if DEBUG
        private const int _PageSize = 2;
#else
        private const int _PageSize = 20;

#endif


        public RacesController(IRaceService raceService, IIdentityMessageService emailService) : base(raceService) 
        {
            _emailService = emailService;
        }

        // GET: Races
        public ActionResult Index(string sortOrder,string distance)
        {
            int page = 1;
            //default to 140.6 
            if (String.IsNullOrEmpty(distance)) distance = "140.6";
            var viewModel = new RaceFilterViewModel(distance:distance,sortOrder:sortOrder);
            var races = viewModel.AvailableRaces;
                    
           
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

           
            viewModel.AvailableRaces = races.ToPagedList(page, _PageSize); //max xx per page
          
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
            var searchString = new StringBuilder();
            if (!String.IsNullOrEmpty(form["SelectedSwimTags"]))
            {
                searchString.Append(form["SelectedSwimTags"]);
            }

            if (!String.IsNullOrEmpty(form["SelectedBikeTags"]))
            {
                if(searchString.Length > 0)
                    searchString.AppendFormat(", {0}", (form["SelectedBikeTags"]));
                else
                    searchString.Append(form["SelectedBikeTags"]);
            }

            if (!String.IsNullOrEmpty(form["SelectedRunTags"]))
            {
                if (searchString.Length > 0)
                    searchString.AppendFormat(", {0}", (form["SelectedRunTags"]));
                else
                    searchString.Append(form["SelectedRunTags"]);
            }

            if(String.IsNullOrEmpty(searchString.ToString()))
                return HttpNotFound();


            var tagIds = searchString.ToString().Split(',').Select(int.Parse).ToList();
            var races = _RaceService.GetRacesByTagId(tagIds);

            var viewModel = new RaceFilterViewModel();
            viewModel.AvailableRaces = races.ToPagedList(pageNumber:1,pageSize: _PageSize); //max xx per page

            return PartialView("_SearchResults", viewModel);
            
        }
        public PartialViewResult ShowRaceRequest()
        {
        
            return PartialView("_RaceRequest", new RequestRaceForm());
        }

        public PartialViewResult RaceDistanceToggle(string distance)
        {
            var viewModel = new RaceFilterViewModel(distance);
        
            int page = 1;
            viewModel.AvailableRaces = viewModel.AvailableRaces.ToPagedList(page, _PageSize); //max xx per page

            return PartialView("~/Views/Shared/_OnePageOfRaces.cshtml", viewModel);
        }

        /// <summary>
        /// Called while paging through athletes. We need to return just the partial view of athletes
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //
        public PartialViewResult DisplayPagedRaces(int page,string sortOrder, SimpleFilterViewModel model)
        {
            var filter = new RaceFilterViewModel(model,sortOrder);
            page = page > 0 ? page : 1;
            filter.AvailableRaces = filter.AvailableRaces.ToPagedList(page, _PageSize); //max xx per page

            return PartialView("~/Views/Shared/_OnePageOfRaces.cshtml", filter);
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
                catch (Exception )
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
                [Bind(Prefix = "Race", Include = "BaseURL,RaceId,ApiName,DisplayName,LongDisplayName,RaceDate,ShortName,Distance")]Race race)
             

        {
            race.Conditions = new RaceConditions();
            race.RaceId = race.RaceId.ToUpper();

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
                [Bind(Prefix ="Race", Include = "RaceId,ApiName,ConditionsId,BaseURL,DisplayName,LongDisplayName,RaceDate,ShortName,Distance")]Race race,
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

            return RedirectToAction("Admin");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Verify(string id)
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

            _RaceService.VerifyRace(race);

            var query = _DBContext.RequestContext.Where(r => r.RaceId == id);
            race.ValidateMessage = "Validated";
            foreach (var cxt in query)
            {
                if(cxt.Expected != cxt.SourceCount)
                {
                    race.ValidateMessage = "Mismatch";
                    break;
                }
            }
            _DBContext.SaveChanges();

  

            return RedirectToAction("Index", "RequestContexts", new {  raceId = id  });
        }


        public ActionResult Aggregate(string id)
        {
            Race race = _DBContext.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }

            var athletes = _RaceService.GetAthletesFromStorage(
                new BasicRaceCriteria
                {
                    SelectedRaceIds = {id },
                    SelectedAgeGroupIds = 
                        _DBContext.AgeGroups.OrderBy(a => a.DisplayOrder).Select(a => a.AgeGroupId).ToList(),
                    SelectedGenderIds = 
                        _DBContext.Genders.Select(g => g.GenderId).ToList()

                }

            );
            var stats = GetStats(athletes, _DBContext.Races.Single(r => r.RaceId == id));


            var aggr = new RaceAggregate();
            aggr.RaceId = race.RaceId;
            aggr.RaceName = race.LongDisplayName;
            aggr.AthleteCount = athletes.Count;
            aggr.DNFCount = stats.DNFCount;
            aggr.MaleCount = athletes.Where(a => a.RequestContext.Gender.Value.Equals("M")).Count();
            aggr.FemaleCount = athletes.Where(a => a.RequestContext.Gender.Value.Equals("F")).Count();


            aggr.SwimFastest = stats.Swim.Min;
            aggr.BikeFastest = stats.Bike.Min;
            aggr.RunFastest = stats.Run.Min;
            aggr.FinishFastest = stats.Finish.Min;

            aggr.SwimSlowest = stats.Swim.Max;
            aggr.BikeSlowest = stats.Bike.Max;
            aggr.RunSlowest = stats.Run.Max;
            aggr.FinishSlowest = stats.Finish.Max;

            aggr.SwimStdDev = stats.Swim.StandDev;
            aggr.BikeStdDev = stats.Bike.StandDev;
            aggr.RunStdDev = stats.Run.StandDev;
            aggr.FinishStdDev = stats.Finish.StandDev;

            aggr.SwimMedian = stats.Swim.Median;
            aggr.BikeMedian = stats.Bike.Median;
            aggr.RunMedian = stats.Run.Median;
            aggr.FinishMedian = stats.Finish.Median;

      
            _DBContext.RacesAggregate.AddOrUpdate(aggr);
            _DBContext.SaveChanges();

            return RedirectToAction("Admin");
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

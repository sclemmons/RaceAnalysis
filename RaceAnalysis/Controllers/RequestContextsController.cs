using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RaceAnalysis.Models;
using RaceAnalysis.SharedQueueMessages;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RequestContextsController : Controller
    {
        private RaceAnalysisDbContext _DBContext = new RaceAnalysisDbContext();
        private CloudQueue _CacheRequestQueue;

        private IRaceService _RaceService;
        public RequestContextsController(IRaceService service)
        {
            _RaceService = service;
        } 

        // GET: RequestContexts
        public ActionResult Index(string raceId)
        {
            IQueryable<RequestContext> query;
            if (raceId != null)
            {
                query = _DBContext.RequestContext.Where(r => r.RaceId == raceId)
                 .OrderBy(r => r.AgeGroupId);


                if (query == null)
                {
                    return HttpNotFound();
                }
              

            }
            else //display all
            {

                query = _DBContext.RequestContext
                    .Include(r => r.AgeGroup).Include(r => r.Gender)
                    .Include(r => r.Race)
                    .OrderBy(r => r.RaceId).ThenBy(r => r.AgeGroup.DisplayName);
               
            }
 
            return View(query.ToList());   
        }

        public ActionResult Search()
        {
            var viewModel = new RequestContextSearchViewModel();
            viewModel.Races = _DBContext.Races.ToList();
            
            return View(viewModel);
        }
        public ActionResult SearchRequestContexts(FormCollection form)
        {
            var searchString = new StringBuilder();
            if (!String.IsNullOrEmpty(form["SelectedRaces"]))
            {
                searchString.Append(form["SelectedRaces"]);
            }


            if (String.IsNullOrEmpty(searchString.ToString()))
                return HttpNotFound();


            var raceIds = searchString.ToString().Split(',').ToList();
            var requestContexts =
                _DBContext.RequestContext.Where(r => raceIds.Contains(r.RaceId))
                .OrderBy(r => r.AgeGroupId);
                  
            return PartialView("_SearchResults", requestContexts.ToList());




        }

        // GET: RequestContexts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContext requestContext = _DBContext.RequestContext.Find(id);
            if (requestContext == null)
            {
                return HttpNotFound();
            }
            return View(requestContext);
        }

        // GET: RequestContexts/Create
        public ActionResult Create()
        {
            ViewBag.AgeGroupId = new SelectList(_DBContext.AgeGroups, "AgeGroupId", "Value");
            ViewBag.GenderId = new SelectList(_DBContext.Genders, "GenderId", "Value");
            ViewBag.RaceId = new SelectList(_DBContext.Races, "RaceId", "RaceId");
            return View();
        }

        // POST: RequestContexts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestContextId,RaceId,AgeGroupId,GenderId,LastRequestedUTC,Status,Instruction,SourceCount")] RequestContext requestContext)
        {
            if (ModelState.IsValid)
            {
                _DBContext.RequestContext.Add(requestContext);
                _DBContext.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgeGroupId = new SelectList(_DBContext.AgeGroups, "AgeGroupId", "Value", requestContext.AgeGroupId);
            ViewBag.GenderId = new SelectList(_DBContext.Genders, "GenderId", "Value", requestContext.GenderId);
            ViewBag.RaceId = new SelectList(_DBContext.Races, "RaceId", "RaceId", requestContext.RaceId);
            return View(requestContext);
        }

        // GET: RequestContexts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContext requestContext = _DBContext.RequestContext.Find(id);
            if (requestContext == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgeGroupId = new SelectList(_DBContext.AgeGroups, "AgeGroupId", "Value", requestContext.AgeGroupId);
            ViewBag.GenderId = new SelectList(_DBContext.Genders, "GenderId", "Value", requestContext.GenderId);
            ViewBag.RaceId = new SelectList(_DBContext.Races, "RaceId", "RaceId", requestContext.RaceId);
            return View(requestContext);
        }

        // POST: RequestContexts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestContextId,RaceId,AgeGroupId,GenderId,LastRequestedUTC,Status,Instruction,SourceCount")] RequestContext requestContext)
        {
            if (ModelState.IsValid)
            {
                _DBContext.Entry(requestContext).State = EntityState.Modified;
                _DBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgeGroupId = new SelectList(_DBContext.AgeGroups, "AgeGroupId", "Value", requestContext.AgeGroupId);
            ViewBag.GenderId = new SelectList(_DBContext.Genders, "GenderId", "Value", requestContext.GenderId);
            ViewBag.RaceId = new SelectList(_DBContext.Races, "RaceId", "RaceId", requestContext.RaceId);
            return View(requestContext);
        }

        // GET: RequestContexts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContext requestContext = _DBContext.RequestContext.Find(id);
            if (requestContext == null)
            {
                return HttpNotFound();
            }
            return View(requestContext);
        }

        // POST: RequestContexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestContext requestContext = _DBContext.RequestContext.Find(id);
            _DBContext.RequestContext.Remove(requestContext);
            _DBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Retry(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //load the full request context rather than using Find
            RequestContext requestContext = _DBContext.RequestContext.
                                Where(rq => rq.RequestContextId == id).SingleOrDefault();
          
            if (requestContext == null)
            {
                return HttpNotFound();
            }

            //Force get from the source since the primary reason for doing this action is to fill our storage.
             requestContext.Instruction = RequestInstruction.ForceSource;
             _DBContext.Entry(requestContext).State = EntityState.Modified;
             _DBContext.SaveChanges();



            TempData["requestContext"] = requestContext;
            return RedirectToAction("DisplayResultsViewFromSource", "TriStats");

          
            //return View(requestContext);
        }

        public async Task<ActionResult> CacheFill(int? id /*request context id*/)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContext requestContext = _DBContext.RequestContext.Find(id);
            if (requestContext == null)
            {
                return HttpNotFound();
            }

            //Force get from the source since the primary reason for doing this action is to fill our storage.
            requestContext.Instruction = RequestInstruction.ForceSource;
            _DBContext.Entry(requestContext).State = EntityState.Modified;
            _DBContext.SaveChanges();
   
            InitializeStorage();
           
            await AddQueueMessages(requestContext);

            return RedirectToAction("Index");
        }

        public ActionResult Validate(int? id /*request context id*/)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestContext requestContext = _DBContext.RequestContext.Find(id);
            if (requestContext == null)
            {
                return HttpNotFound();
            }
            _RaceService.VerifyRequestContext(requestContext);
            
            return RedirectToAction("Index");
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
            _CacheRequestQueue = queueClient.GetQueueReference("triathletepullrequest");
            _CacheRequestQueue.CreateIfNotExists();

        }
        private async Task AddQueueMessages(RequestContext req)
        {
     
            var msg = new FetchTriathletesMessage()
            {
                RaceId = req.RaceId,
                AgegroupIds = new int[] { req.AgeGroupId },
                GenderIds = new int[] { req.GenderId }
            };
            var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(msg));
            await _CacheRequestQueue.AddMessageAsync(queueMessage);
              
            
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _DBContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

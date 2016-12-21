using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RaceAnalysis.Models;
using System;

namespace RaceAnalysis.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RaceConditionsController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        // GET: RaceConditions
        public ActionResult Index()
        {
            return View(db.RaceConditions.ToList());
        }

        // GET: RaceConditions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceConditions raceConditions = db.RaceConditions.Find(id);
            if (raceConditions == null)
            {
                return HttpNotFound();
            }
            return View(raceConditions);
        }

        // GET: RaceConditions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RaceConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RaceConditionsId,SwimLayout,BikeLayout,RunLayout")] RaceConditions raceConditions)
        {
            if (ModelState.IsValid)
            {
                db.RaceConditions.Add(raceConditions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(raceConditions);
        }

        // GET: RaceConditions/Edit/5  NOTE, this is the raceId, not the conditionsId
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var race = db.Races.Find(id);
            if (race == null)
            {
                return HttpNotFound();
            }
            var viewModel = new RaceViewModel();
            viewModel.Race = race;
            viewModel.Tags = db.Tags.ToList();
            return View(viewModel);
        }

        // POST: RaceConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(
           // [Bind(Prefix="Race.Conditions",Include = "RaceConditionsId,SwimLayout,BikeLayout,RunLayout")] RaceConditions raceConditions)
        public ActionResult Edit(SimpleRaceConditionsViewModel viewModel,int? raceId )
        {
        
            var race = db.Races.Find(raceId);
            if (race == null)
            {
                return HttpNotFound();
            }
            
            Save(race.Conditions, viewModel);

            return RedirectToAction("Index");
        }
        private void Save(RaceConditions conditions, SimpleRaceConditionsViewModel viewModel)
        {
            foreach(string s in viewModel.selectedSwimLayout)
            {
          
               Tag tag;
               int tagId;
               bool result = Int32.TryParse(s, out tagId);
                //tag is either a tag id or a new tag value that the user typed
                if(result)
                {
                    tag = db.Tags.Find(tagId);

                }
                else //a new tag
                {
                    tag = new Tag
                    {
                        Value = s
                    };
                
                    db.Tags.Add(tag);
                    db.SaveChanges();
                    
                }
                var rcTag = conditions.SwimLayout.Where(m => m.TagId == tagId).Single();
                if (rcTag == null)
                {
                    rcTag = new RaceConditionTag
                    {
                        RaceConditions = conditions,
                        Tag = tag,
                        Count = 1
                    };
                    db.RaceConditionTags.Add(rcTag);
                    conditions.SwimLayout.Add(rcTag);
                }
                else
                {
                    rcTag.Count += 1;
                }

            }


                //db.Entry(conditions).State = EntityState.Modified;
                db.SaveChanges();


        }
    

        // GET: RaceConditions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceConditions raceConditions = db.RaceConditions.Find(id);
            if (raceConditions == null)
            {
                return HttpNotFound();
            }
            return View(raceConditions);
        }

        // POST: RaceConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RaceConditions raceConditions = db.RaceConditions.Find(id);
            db.RaceConditions.Remove(raceConditions);
            db.SaveChanges();
            return RedirectToAction("Index");
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

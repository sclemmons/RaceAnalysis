using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RaceAnalysis.Models;
using System;
using System.Collections.Generic;

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
            var viewModel = new RaceConditionsViewModel();
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

            return RedirectToAction("admin", "races");
        }

        private void Save(RaceConditions conditions, SimpleRaceConditionsViewModel viewModel)
        {
            Save(conditions, conditions.SwimLayout, viewModel.selectedSwimLayout,TagType.SwimLayout);
            Save(conditions, conditions.SwimMedium, viewModel.selectedSwimMedium,TagType.SwimMedium);
            Save(conditions, conditions.SwimOther, viewModel.selectedSwimOther,TagType.SwimOther);
            Save(conditions, conditions.SwimWeather, viewModel.selectedSwimWeather,TagType.SwimWeather);


            Save(conditions, conditions.BikeLayout, viewModel.selectedBikeLayout,TagType.BikeLayout);
            Save(conditions, conditions.BikeMedium, viewModel.selectedBikeMedium, TagType.BikeMedium);
            Save(conditions, conditions.BikeOther, viewModel.selectedBikeOther, TagType.BikeOther);
            Save(conditions, conditions.BikeWeather, viewModel.selectedBikeWeather,TagType.BikeWeather);


            Save(conditions, conditions.RunLayout, viewModel.selectedRunLayout,TagType.RunLayout);
            Save(conditions, conditions.RunMedium, viewModel.selectedRunMedium,TagType.RunMedium);
            Save(conditions, conditions.RunOther, viewModel.selectedRunOther,TagType.RunOther);
            Save(conditions, conditions.RunWeather, viewModel.selectedRunWeather,TagType.RunWeather);




        }
        private void Save(RaceConditions conditions,List<RaceConditionTag> persistedList, List<String> selectedValues,TagType tagType)
        {
            if (selectedValues == null)
            {
                if (persistedList.Count > 0)
                    selectedValues = new List<string>(); //the reason we continue is in the case wehre all items have been removed from list and we need to delete them
                else
                    return; //we have no further business to do
            }
            //get the tagIds that have previously been saved and convert them to strings in order to compare them
            var savedList = persistedList.Select(t => t.TagId).ToList().ConvertAll<string>(delegate (int i) { return i.ToString(); });
         
            //get the unique items that should be added
            var newItems = selectedValues.Except(savedList).ToList();

            //find the items that were removed
            var deletedItems = savedList.Except(selectedValues).ToList();

            foreach (string id in deletedItems)
            {
                var rcTag = persistedList.Where(m => m.TagId == Int32.Parse(id)).SingleOrDefault();
                persistedList.Remove(rcTag);
                db.RaceConditionTags.Remove(rcTag);
              
            }

            foreach (string s in newItems)
            {

                Tag tag;
                int tagId;
                bool result = Int32.TryParse(s, out tagId);
                //tag is either a tag id or a new tag value that the user typed
                if (result)
                {
                    tag = db.Tags.Find(tagId);

                }
                else //a new tag
                {
                    tag = new Tag
                    {
                        Value = s,
                        Type = tagType
                        
                    };

                    db.Tags.Add(tag);
                    db.SaveChanges();

                }


                var rcTag = new RaceConditionTag  //because we already filtered out new items, we should always need to create
                {
                    RaceConditions = conditions,
                    RaceConditionsId = conditions.RaceConditionsId, //when this is a new racecondition, this id is not being populated into the table, so we're forcing it
                    Tag = tag,
                    TagId = tag.TagId,
                    Count = 1
                };
                db.RaceConditionTags.Add(rcTag);
                persistedList.Add(rcTag);
            }
                         
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

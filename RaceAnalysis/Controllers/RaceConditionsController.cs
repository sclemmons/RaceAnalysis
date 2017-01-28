using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RaceAnalysis.Models;
using System;
using System.Collections.Generic;

namespace RaceAnalysis.Controllers
{
    public class RaceConditionsController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();



        // GET: RaceConditions
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.RaceConditions.ToList());
        }

        // GET: RaceConditions/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id /* race condition id*/)
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

        // GET: RaceConditions/Create/5  NOTE, this is the raceId, not the conditionsId
        public ActionResult Create(string id /*raceid*/)
        {
            if (String.IsNullOrEmpty(id))
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
            viewModel.Split = "swim";
            viewModel.Tags = db.Tags.Where(
                        t => t.Type == TagType.SwimLayout 
                        || t.Type == TagType.SwimMedium
                        || t.Type == TagType.SwimWeather
                        || t.Type == TagType.SwimOther
                        ).ToList();
            return View(viewModel);
        }

        public ActionResult Vote(string tagIds,List<String> newTags, string raceId, string split)
        {
            ModelState.Clear(); //we must do this so that the input values can change on each partial view

            var race = db.Races.Find(raceId);
            if (race == null)
            {
                return HttpNotFound();
            }

        
            //create new tags, if any
            var newTagIds = SaveNewTags(newTags, split);

            // existing tags, we need to increment count
            var tagList = String.IsNullOrEmpty(tagIds) ? new List<int>() : tagIds.Split(',').Select(Int32.Parse).ToList();
            tagList.AddRange(newTagIds);
            IncrementTagCount(race.Conditions, tagList);

            //setup for next partial view
            var viewModel = new RaceConditionsViewModel();
            viewModel.Race = race;
            switch (split)
            {

                case "swim":
                    //setup for bike:
                       viewModel.Split = "bike";
                        viewModel.Tags = db.Tags.Where(
                          t => t.Type == TagType.BikeLayout
                          || t.Type == TagType.BikeMedium
                          || t.Type == TagType.BikeWeather
                          || t.Type == TagType.BikeOther
                          ).ToList();
                    break;
                case "bike":
                    //setup for run:
                    viewModel.Split = "run";
                    viewModel.Tags = db.Tags.Where(
                      t => t.Type == TagType.RunLayout
                      || t.Type == TagType.RunMedium
                      || t.Type == TagType.RunWeather
                      || t.Type == TagType.RunOther
                      ).ToList();
                    break;
                case "run":
                    return Json("<h3>Thank You for your contributions!</h3>");
                    
            }


            
            return PartialView("_Splits",viewModel);
        }

        private List<int> SaveNewTags(List<string> userTags, string type)
        {
            var newTagIds = new List<int>();

            if (userTags == null)
                return newTagIds;
                      

            TagType tagType;
            switch (type)
            {
                case "swim":
                    tagType = TagType.SwimOther;
                    break;

                case "bike":
                    tagType = TagType.BikeOther;
                    break;
                case "run":
                    tagType = TagType.RunOther;
                    break;

                default:
                    throw new Exception("Tag type not found");
            }
            foreach (string s in userTags)
            {
                Tag tag;
                int tagId;
                bool result = Int32.TryParse(s, out tagId);
                //tag is either an existing tag id or a new tag value that the user typed
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
                }
                

                db.Tags.Add(tag);
                db.SaveChanges();
                newTagIds.Add(tag.TagId);

            }
            return newTagIds;

            
        }
        private void IncrementTagCount(RaceConditions conditions,List<int> tagIds)
        {
            if (tagIds == null)
                return;

            
            foreach (var id in tagIds)
            {
                Tag tag = db.Tags.Find(id);
                RaceConditionTag rcTag;
                switch (tag.Type)
                {
                    case TagType.SwimLayout:
                        rcTag = conditions.SwimLayout.Find(t => t.TagId == id);
                        break;
                    case TagType.SwimMedium:
                        rcTag = conditions.SwimMedium.Find(t => t.TagId == id);
                        break;
                    case TagType.SwimWeather:
                        rcTag = conditions.SwimWeather.Find(t => t.TagId == id);
                        break;
                    case TagType.SwimOther:
                        rcTag = conditions.SwimOther.Find(t => t.TagId == id);
                        break;

                    case TagType.BikeLayout:
                        rcTag = conditions.BikeLayout.Find(t => t.TagId == id);
                        break;
                    case TagType.BikeMedium:
                        rcTag = conditions.BikeMedium.Find(t => t.TagId == id);
                        break;
                    case TagType.BikeWeather:
                        rcTag = conditions.BikeWeather.Find(t => t.TagId == id);
                        break;
                    case TagType.BikeOther:
                        rcTag = conditions.BikeOther.Find(t => t.TagId == id);
                        break;

                    case TagType.RunLayout:
                        rcTag = conditions.RunLayout.Find(t => t.TagId == id);
                        break;
                    case TagType.RunMedium:
                        rcTag = conditions.RunMedium.Find(t => t.TagId == id);
                        break;
                    case TagType.RunWeather:
                        rcTag = conditions.RunWeather.Find(t => t.TagId == id);
                        break;
                    case TagType.RunOther:
                        rcTag = conditions.RunOther.Find(t => t.TagId == id);
                        break;
                    default:
                        throw new Exception("Tag Condition not found");
                }
                if(rcTag == null) //a new tag added by user
                {
                    rcTag = new RaceConditionTag  
                    {
                        RaceConditions = conditions,
                        RaceConditionsId = conditions.RaceConditionsId, //when this is a new racecondition, this id is not being populated into the table, so we're forcing it
                        Tag = tag,
                        TagId = tag.TagId,
                        Count = 1
                    };
                    switch (tag.Type)
                    {
                        case TagType.SwimLayout:
                             conditions.SwimLayout.Add(rcTag);
                            break;
                        case TagType.SwimMedium:
                            conditions.SwimMedium.Add(rcTag);
                            break;
                        case TagType.SwimWeather:
                            conditions.SwimWeather.Add(rcTag);
                            break;
                        case TagType.SwimOther:
                            conditions.SwimOther.Add(rcTag);
                            break;

                        case TagType.BikeLayout:
                            conditions.BikeLayout.Add(rcTag);
                            break;
                        case TagType.BikeMedium:
                            conditions.BikeMedium.Add(rcTag);
                            break;
                        case TagType.BikeWeather:
                            conditions.BikeWeather.Add(rcTag);
                            break;
                        case TagType.BikeOther:
                            conditions.BikeOther.Add(rcTag);
                            break;

                        case TagType.RunLayout:
                            conditions.RunLayout.Add(rcTag);
                            break;
                        case TagType.RunMedium:
                            conditions.RunMedium.Add(rcTag);
                            break;
                        case TagType.RunWeather:
                            conditions.RunWeather.Add(rcTag);
                            break;
                        case TagType.RunOther:
                            conditions.RunOther.Add(rcTag);
                            break;
                        default:
                            throw new Exception("Tag Condition not found");
                    }

                    db.RaceConditionTags.Add(rcTag);
                    

                }
                else
                {
                    rcTag.Count += 1;
                }

               
                db.SaveChanges();
            }
            
           
           
        }

        // POST: RaceConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
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
        [Authorize(Roles = "Admin")]
        //public ActionResult Edit(
        // [Bind(Prefix="Race.Conditions",Include = "RaceConditionsId,SwimLayout,BikeLayout,RunLayout")] RaceConditions raceConditions)
        public ActionResult Edit(SimpleRaceConditionsViewModel viewModel,string raceId )
        {
        
            var race = db.Races.Find(raceId);
            if (String.IsNullOrEmpty(raceId))
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
                db.RaceConditionTags.Add(rcTag); //add the rc tag to the database
                //persistedList.Add(rcTag); //relate the rc tag to the condition-specific-list
                //tag.RaceConditionTags.Add(rcTag);
                

            }
                         
             db.SaveChanges();


        }

        // GET: RaceConditions/Edit/5  NOTE, this is the raceId, not the conditionsId
        public ActionResult ShowRaceConditions(string id)
        {
            if (String.IsNullOrEmpty(id))
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
            return PartialView("~/Views/Shared/_RaceConditions.cshtml",viewModel);
        }

        [Authorize(Roles = "Admin")]
        // GET: RaceConditions/Delete/5
        public ActionResult Delete(int? id /*racecondition id */) 
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
        public ActionResult DeleteConfirmed(int id /*race condition id*/)
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

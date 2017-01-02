using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RaceAnalysis.Models;

namespace RaceAnalysis.Controllers
{
    public class TagsController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        // GET: Tags
        public ActionResult Index()
        {
            List<Tag> tags = db.Tags.OrderBy(t => t.Type).ToList();

            return View(tags);
        }

        // GET: Tags/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TagId,Type,Value")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tag);
        }

        // GET: Tags/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TagId,Type,Value")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = db.Tags.Find(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        public ActionResult EditAll()
        {
            var viewModel = new TagViewModel();
            viewModel.Tags = db.Tags.ToList();

            return View("EditAll",viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(
        // [Bind(Prefix="Race.Conditions",Include = "RaceConditionsId,SwimLayout,BikeLayout,RunLayout")] RaceConditions raceConditions)
        public ActionResult EditAll(SimpleRaceConditionsViewModel postedModel)
        {

            if (ModelState.IsValid)
            {
                Save(postedModel);
                return RedirectToAction("Index");
            }

            var tagviewModel = new TagViewModel();
            tagviewModel.Tags = db.Tags.ToList();

            return View("EditAll", tagviewModel);
        }

        private void Save(SimpleRaceConditionsViewModel viewModel)
        {
            Save(viewModel.selectedSwimLayout, TagType.SwimLayout);
            Save(viewModel.selectedSwimMedium, TagType.SwimMedium);
            Save(viewModel.selectedSwimOther, TagType.SwimOther);
            Save(viewModel.selectedSwimWeather, TagType.SwimWeather);


            Save(viewModel.selectedBikeLayout, TagType.BikeLayout);
            Save(viewModel.selectedBikeMedium, TagType.BikeMedium);
            Save(viewModel.selectedBikeOther, TagType.BikeOther);
            Save(viewModel.selectedBikeWeather, TagType.BikeWeather);


            Save(viewModel.selectedRunLayout, TagType.RunLayout);
            Save(viewModel.selectedRunMedium, TagType.RunMedium);
            Save(viewModel.selectedRunOther, TagType.RunOther);
            Save(viewModel.selectedRunWeather, TagType.RunWeather);

        }
        private void Save(List<String> selectedValues, TagType tagType)
        {
            if (selectedValues == null)
            {
                    return; //we have no further business to do
            }
            foreach (string s in selectedValues)
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
                }
            }

            db.SaveChanges();


        }





        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
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

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
    public class AgeGroupAggregatesController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        // GET: AgeGroupAggregates
        public ActionResult Index()
        {
            var ageGroupAggregates = db.AgeGroupAggregates.Include(a => a.AgeGroup).Include(a => a.Gender).Include(a => a.Race);
            return View(ageGroupAggregates.ToList());
        }
        public ActionResult List()
        {
            return View();
        }

        // GET: AgeGroupAggregates/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgeGroupAggregate ageGroupAggregate = db.AgeGroupAggregates.Find(id);
            if (ageGroupAggregate == null)
            {
                return HttpNotFound();
            }
            return View(ageGroupAggregate);
        }

        // GET: AgeGroupAggregates/Create
        public ActionResult Create()
        {
            ViewBag.AgeGroupId = new SelectList(db.AgeGroups, "AgeGroupId", "Value");
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Value");
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL");
            return View();
        }

        // POST: AgeGroupAggregates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RaceId,AgeGroupId,GenderId,AthleteCount,DNFCount,MaleCount,FemaleCount,SwimMedian,BikeMedian,RunMedian,FinishMedian,SwimFastest,BikeFastest,RunFastest,FinishFastest,SwimSlowest,BikeSlowest,RunSlowest,FinishSlowest,SwimStdDev,BikeStdDev,RunStdDev,FinishStdDev")] AgeGroupAggregate ageGroupAggregate)
        {
            if (ModelState.IsValid)
            {
                db.AgeGroupAggregates.Add(ageGroupAggregate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgeGroupId = new SelectList(db.AgeGroups, "AgeGroupId", "Value", ageGroupAggregate.AgeGroupId);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Value", ageGroupAggregate.GenderId);
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL", ageGroupAggregate.RaceId);
            return View(ageGroupAggregate);
        }

        // GET: AgeGroupAggregates/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgeGroupAggregate ageGroupAggregate = db.AgeGroupAggregates.Find(id);
            if (ageGroupAggregate == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgeGroupId = new SelectList(db.AgeGroups, "AgeGroupId", "Value", ageGroupAggregate.AgeGroupId);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Value", ageGroupAggregate.GenderId);
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL", ageGroupAggregate.RaceId);
            return View(ageGroupAggregate);
        }

        // POST: AgeGroupAggregates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RaceId,AgeGroupId,GenderId,AthleteCount,DNFCount,MaleCount,FemaleCount,SwimMedian,BikeMedian,RunMedian,FinishMedian,SwimFastest,BikeFastest,RunFastest,FinishFastest,SwimSlowest,BikeSlowest,RunSlowest,FinishSlowest,SwimStdDev,BikeStdDev,RunStdDev,FinishStdDev")] AgeGroupAggregate ageGroupAggregate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ageGroupAggregate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgeGroupId = new SelectList(db.AgeGroups, "AgeGroupId", "Value", ageGroupAggregate.AgeGroupId);
            ViewBag.GenderId = new SelectList(db.Genders, "GenderId", "Value", ageGroupAggregate.GenderId);
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL", ageGroupAggregate.RaceId);
            return View(ageGroupAggregate);
        }

        // GET: AgeGroupAggregates/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgeGroupAggregate ageGroupAggregate = db.AgeGroupAggregates.Find(id);
            if (ageGroupAggregate == null)
            {
                return HttpNotFound();
            }
            return View(ageGroupAggregate);
        }

        // POST: AgeGroupAggregates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AgeGroupAggregate ageGroupAggregate = db.AgeGroupAggregates.Find(id);
            db.AgeGroupAggregates.Remove(ageGroupAggregate);
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

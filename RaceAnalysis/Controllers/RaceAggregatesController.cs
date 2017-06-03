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
    public class RaceAggregatesController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        // GET: RaceAggregates
        public ActionResult Index()
        {
            var racesAggregate = db.RacesAggregates.Include(r => r.Race);
            return View(racesAggregate.ToList());
        }
        public ActionResult List()
        {
            return View();
        }

        // GET: RaceAggregates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceAggregate raceAggregate = db.RacesAggregates.Find(id);
            if (raceAggregate == null)
            {
                return HttpNotFound();
            }
            return View(raceAggregate);
        }

        // GET: RaceAggregates/Create
        public ActionResult Create()
        {
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL");
            return View();
        }

        // POST: RaceAggregates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RaceAggregateId,RaceId,AthleteCount,DNFCount,MaleCount,FemaleCount,SwimMedian,BikeMedian,RunMedian,FinishMedian,SwimFastest,BikeFastest,RunFastest,FinishFastest,SwimSlowest,BikeSlowest,RunSlowest,FinishSlowest,SwimStdDev,BikeStdDev,RunStdDev,FinishStdDev")] RaceAggregate raceAggregate)
        {
            if (ModelState.IsValid)
            {
                db.RacesAggregates.Add(raceAggregate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL", raceAggregate.RaceId);
            return View(raceAggregate);
        }

        // GET: RaceAggregates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceAggregate raceAggregate = db.RacesAggregates.Find(id);
            if (raceAggregate == null)
            {
                return HttpNotFound();
            }
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL", raceAggregate.RaceId);
            return View(raceAggregate);
        }

        // POST: RaceAggregates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RaceAggregateId,RaceId,AthleteCount,DNFCount,MaleCount,FemaleCount,SwimMedian,BikeMedian,RunMedian,FinishMedian,SwimFastest,BikeFastest,RunFastest,FinishFastest,SwimSlowest,BikeSlowest,RunSlowest,FinishSlowest,SwimStdDev,BikeStdDev,RunStdDev,FinishStdDev")] RaceAggregate raceAggregate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(raceAggregate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RaceId = new SelectList(db.Races, "RaceId", "BaseURL", raceAggregate.RaceId);
            return View(raceAggregate);
        }

        // GET: RaceAggregates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RaceAggregate raceAggregate = db.RacesAggregates.Find(id);
            if (raceAggregate == null)
            {
                return HttpNotFound();
            }
            return View(raceAggregate);
        }

        // POST: RaceAggregates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RaceAggregate raceAggregate = db.RacesAggregates.Find(id);
            db.RacesAggregates.Remove(raceAggregate);
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

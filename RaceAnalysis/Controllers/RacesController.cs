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
    public class RacesController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

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

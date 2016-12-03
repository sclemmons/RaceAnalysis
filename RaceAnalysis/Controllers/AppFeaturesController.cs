using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RaceAnalysis.Models;
using System.Threading.Tasks;

namespace RaceAnalysis
{
    public class AppFeaturesController : Controller
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        // GET: AppFeatures
        public ActionResult Index()
        {
            return View(db.AppFeatures.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> UpVote(int id, bool up)
        {
         
            AppFeature appFeature = db.AppFeatures.Find(id);
            if(appFeature == null)
                return Json(new { status = "Error" });
           
            if (up)
                ++appFeature.VoteCount;
            else
                --appFeature.VoteCount;

                
            db.Entry(appFeature).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json(new { status = "Success" });
        }

        // GET: AppFeatures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppFeature appFeature = db.AppFeatures.Find(id);
            if (appFeature == null)
            {
                return HttpNotFound();
            }
            return View(appFeature);
        }

        // GET: AppFeatures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppFeatureId,Category,Name,Description,State,VoteCount")] AppFeature appFeature)
        {
            if (ModelState.IsValid)
            {
                db.AppFeatures.Add(appFeature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appFeature);
        }

        // GET: AppFeatures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppFeature appFeature = db.AppFeatures.Find(id);
            if (appFeature == null)
            {
                return HttpNotFound();
            }
            return View(appFeature);
        }

        // POST: AppFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppFeatureId,Category,Name,Description,State,VoteCount")] AppFeature appFeature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appFeature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appFeature);
        }

        // GET: AppFeatures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppFeature appFeature = db.AppFeatures.Find(id);
            if (appFeature == null)
            {
                return HttpNotFound();
            }
            return View(appFeature);
        }

        // POST: AppFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppFeature appFeature = db.AppFeatures.Find(id);
            db.AppFeatures.Remove(appFeature);
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

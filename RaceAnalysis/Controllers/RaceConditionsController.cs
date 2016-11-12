using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using RaceAnalysis.Models;

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
        public ActionResult Create([Bind(Include = "RaceConditionsId,SwimGeneral,BikeGeneral,RunGeneral")] RaceConditions raceConditions)
        {
            if (ModelState.IsValid)
            {
                db.RaceConditions.Add(raceConditions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(raceConditions);
        }

        // GET: RaceConditions/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: RaceConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RaceConditionsId,SwimGeneral,BikeGeneral,RunGeneral")] RaceConditions raceConditions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(raceConditions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(raceConditions);
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

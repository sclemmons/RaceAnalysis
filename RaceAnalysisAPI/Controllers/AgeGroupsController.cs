using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RaceAnalysis.Models;

namespace RaceAnalysisAPI.Controllers
{
    public class AgeGroupsController : ApiController
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();

        // GET: api/AgeGroups
        public IQueryable<AgeGroup> GetAgeGroups()
        {
            return db.AgeGroups;
        }

        // GET: api/AgeGroups/5
        [ResponseType(typeof(AgeGroup))]
        public async Task<IHttpActionResult> GetAgeGroup(int id)
        {
            AgeGroup ageGroup = await db.AgeGroups.FindAsync(id);
            if (ageGroup == null)
            {
                return NotFound();
            }

            return Ok(ageGroup);
        }

        // PUT: api/AgeGroups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAgeGroup(int id, AgeGroup ageGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ageGroup.AgeGroupId)
            {
                return BadRequest();
            }

            db.Entry(ageGroup).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgeGroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AgeGroups
        [ResponseType(typeof(AgeGroup))]
        public async Task<IHttpActionResult> PostAgeGroup(AgeGroup ageGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AgeGroups.Add(ageGroup);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ageGroup.AgeGroupId }, ageGroup);
        }

        // DELETE: api/AgeGroups/5
        [ResponseType(typeof(AgeGroup))]
        public async Task<IHttpActionResult> DeleteAgeGroup(int id)
        {
            AgeGroup ageGroup = await db.AgeGroups.FindAsync(id);
            if (ageGroup == null)
            {
                return NotFound();
            }

            db.AgeGroups.Remove(ageGroup);
            await db.SaveChangesAsync();

            return Ok(ageGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AgeGroupExists(int id)
        {
            return db.AgeGroups.Count(e => e.AgeGroupId == id) > 0;
        }
    }
}
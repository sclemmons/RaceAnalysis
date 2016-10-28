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
using RaceAnalysis.DAL;

namespace RaceAnalysisAPI.Controllers
{
    public class TriathletesController : ApiController
    {
        private RaceAnalysisDbContext _DBContext = new RaceAnalysisDbContext();
        private TriathletesDAL _DAL;

        public TriathletesController()
        {
             _DAL = new TriathletesDAL(_DBContext);
        }


        // GET: api/Triathletes
        /// <summary>
        /// Get all triathletes for a specific race, agegroups, genders
        /// http://localhost:52873/api/Triathletes?raceId=17&agegroupIds=72,76&genderIds=9,10
        /// </summary>
        /// <param name="raceId"></param>
        /// <param name="agegroupIds"></param>
        /// <param name="genderIds"></param>
        /// <returns></returns>
        public IQueryable<Triathlete> GetTriathletes(int raceId, string agegroupIds, string genderIds)
        {
          
            var a = Array.ConvertAll(agegroupIds.Split(','), int.Parse);
            var g = Array.ConvertAll(genderIds.Split(','), int.Parse);

            List<Triathlete> athletes = _DAL.GetAthletes(new int[] { raceId }, a, g);

            return athletes.AsQueryable();
        }



        // PUT: api/Triathletes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTriathlete(string id, Triathlete triathlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != triathlete.Link)
            {
                return BadRequest();
            }

            _DBContext.Entry(triathlete).State = EntityState.Modified;

            try
            {
                await _DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TriathleteExists(id))
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

        // POST: api/Triathletes
        [ResponseType(typeof(Triathlete))]
        public async Task<IHttpActionResult> PostTriathlete(Triathlete triathlete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _DBContext.Triathletes.Add(triathlete);

            try
            {
                await _DBContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TriathleteExists(triathlete.Link))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = triathlete.Link }, triathlete);
        }

        // DELETE: api/Triathletes/5
        [ResponseType(typeof(Triathlete))]
        public async Task<IHttpActionResult> DeleteTriathlete(string id)
        {
            Triathlete triathlete = await _DBContext.Triathletes.FindAsync(id);
            if (triathlete == null)
            {
                return NotFound();
            }

            _DBContext.Triathletes.Remove(triathlete);
            await _DBContext.SaveChangesAsync();

            return Ok(triathlete);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _DBContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TriathleteExists(string id)
        {
            return _DBContext.Triathletes.Count(e => e.Link == id) > 0;
        }
    }
}
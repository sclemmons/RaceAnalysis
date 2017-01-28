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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using RaceAnalysisAPI.Dtos;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysisAPI.Controllers
{
    public class RacesController : ApiController
    {
        private RaceAnalysisDbContext db = new RaceAnalysisDbContext();
        internal static Action<Exception> OnException { get; set; }
        protected IRaceService _RaceService;

        private RaceAnalysisDbContext _DBContext = new RaceAnalysisDbContext();
   
             
        public RacesController(IRaceService service)
        {
            _RaceService = service;
            Mapper.Initialize(MappingConfiguration.Configure);
            
        }
        // GET: api/Races
        public IQueryable<RaceDto> GetRaces()
        {
            var races = db.Races.Include("Race").UseAsDataSource(Mapper.Instance)
                .For<RaceDto>()
                .OrderBy(r => r.NewDisplayName);


            return races;

        }

        // GET: api/Races/5
        [ResponseType(typeof(Race))]
        public async Task<IHttpActionResult> GetRace(int id)
        {
            Race race = await db.Races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }

            return Ok(race);
        }

        // PUT: api/Races/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRace(string id, Race race)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != race.RaceId)
            {
                return BadRequest();
            }

            db.Entry(race).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceExists(id))
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

        // POST: api/Races
        [ResponseType(typeof(Race))]
        public async Task<IHttpActionResult> PostRace(Race race)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Races.Add(race);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = race.RaceId }, race);
        }

        // DELETE: api/Races/5
        [ResponseType(typeof(Race))]
        public async Task<IHttpActionResult> DeleteRace(int id)
        {
            Race race = await db.Races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }

            db.Races.Remove(race);
            await db.SaveChangesAsync();

            return Ok(race);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RaceExists(string id)
        {
            return db.Races.Count(e => e.RaceId == id) > 0;
        }
    }
}
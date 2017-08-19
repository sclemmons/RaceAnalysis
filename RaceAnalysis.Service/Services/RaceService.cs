using System;
using System.Collections.Generic;
using System.Linq;
using RaceAnalysis.Models;
using Castle.DynamicLinqQueryBuilder;
using System.Data.Entity.Migrations;
using RaceAnalysis.Rest;
using RestSharp;
using RaceAnalysis.Service.Interfaces;
using RaceAnalysis.ServiceSupport;
using System.Diagnostics;

namespace RaceAnalysis.Service
{
    public class RaceService : IRaceService
    {
        private RaceAnalysisDbContext _DBContext;
        public RaceService(RaceAnalysisDbContext db)
        {
            _DBContext = db;
        }

        /// <summary>
        /// get athletes given the criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Triathlete> GetAthletes(IRaceCriteria criteria,bool useCache=true)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Triathlete> cachedAtheletes = null;

            if (useCache)
            {
                cachedAtheletes = CacheService.Instance.GetAthletes(criteria);
            }
            if (cachedAtheletes != null && cachedAtheletes.Count > 0)
            {
                stopwatch.Stop();

                Trace.TraceInformation(String.Format("GetAthletsFromCache took: {0} ", stopwatch.Elapsed.ToString()));

                return cachedAtheletes;
            }
           

            List<Triathlete> allAthletes = new List<Triathlete>();

            //create a requestContext for each combination. 
            foreach (string raceId in criteria.SelectedRaceIds)
            {
                foreach (int ageId in criteria.SelectedAgeGroupIds)
                {
                    foreach (int genderId in criteria.SelectedGenderIds)
                    {
                        RequestContext reqContext = GetRequestContext(raceId, ageId, genderId);
                        List<Triathlete> athletesInReqContext = new List<Triathlete>();
                        if (reqContext == null ||
                              reqContext.Instruction == RequestInstruction.ForceSource ||
                              useCache== false //force getting from source

                           )
                        {
                            if (reqContext == null)
                                reqContext = CreateRequestContext(raceId, ageId, genderId);
                            else
                            { //we need to remove any athletes that might be there under that context to avoid dupes
                              // DeleteAthletes(reqContext.RequestContextId);
                            }
                            //get from source 
                            athletesInReqContext = GetAthletesFromSource(reqContext);
                        }
                        else
                        {
                            athletesInReqContext = GetAthletesFromDb(reqContext);
                        }

                        if (athletesInReqContext.Count > 0)
                        {
                            allAthletes.AddRange(athletesInReqContext);
                        }
                    }

                }
            }

            var athletes =  allAthletes.OrderBy(t => t.Finish).ToList();

            stopwatch.Stop();
            Trace.TraceInformation(String.Format("GetAthletes without cache took: {0} ", stopwatch.Elapsed.ToString()));

            CacheService.Instance.PopulateAthletes(criteria, athletes);

            return athletes;
        }


        /***************************************
         * GetAthletes()
         * Retrieve the athletes with the given request values
         * ****************************************/
        public List<Triathlete> GetAthletes(IRaceCriteria criteria, IDurationFilter filter)
        {
            var allAthletes = GetAthletes(criteria);

            //filter these athletes
            //in the future we may inject the Provider, but for now create it ...
            return new BasicFilterProvider(allAthletes, filter).GetAthletes();

        }

        /***************************************
      * GetAthletes()
      * Retrieve the athletes from storage only. This is basically for testing to see if we are successfully pulling and storing.
      * ****************************************/
        public List<Triathlete> GetAthletesFromStorage(IRaceCriteria criteria)
        {

            List<Triathlete> allAthletes = new List<Triathlete>();



            //create a requestContext for each combination. 
            foreach (string raceId in criteria.SelectedRaceIds)
            {
                foreach (int ageId in criteria.SelectedAgeGroupIds)
                {
                    foreach (int genderId in criteria.SelectedGenderIds)
                    {
                        RequestContext reqContext = GetRequestContext(raceId, ageId, genderId);
                        List<Triathlete> athletesInReqContext = new List<Triathlete>();
                        if (reqContext == null)
                        {
                            //don't do anything. We have not stored these athletes
                          
                        }
                        else
                        {
                            athletesInReqContext = GetAthletesFromDb(reqContext);
                        }

                        if (athletesInReqContext.Count > 0)
                        {
                            allAthletes.AddRange(athletesInReqContext);
                        }
                    }

                }
            }

            return allAthletes.OrderBy(t => t.Finish).ToList();

        }
        public List<Triathlete> GetAthletesFromSource(IRaceCriteria criteria)
        {
            List<Triathlete> allAthletes = new List<Triathlete>();

            //create a requestContext for each combination. 
            foreach (string raceId in criteria.SelectedRaceIds)
            {
                foreach (int ageId in criteria.SelectedAgeGroupIds)
                {
                    foreach (int genderId in criteria.SelectedGenderIds)
                    {
                        RequestContext reqContext = GetRequestContext(raceId, ageId, genderId);
                        List<Triathlete> athletesInReqContext = new List<Triathlete>();
                        if (reqContext == null)
                        {
                            //don't do anything. We have not stored these athletes

                        }
                        else
                        {
                            athletesInReqContext = GetAthletesFromSource(reqContext);

                        }

                        if (athletesInReqContext.Count > 0)
                        {
                            allAthletes.AddRange(athletesInReqContext);
                        }
                    }

                }
            }

            return allAthletes.OrderBy(t => t.Finish).ToList();

        }


        public void VerifyRequestContext(RequestContext reqContext)
        {
            string baseUrl = reqContext.Race.BaseURL;

            IronmanClient sourceClient;  //we'll change this to a factory pattern if we have more than these two classes

            string apiName = reqContext.Race.ApiName == null ? "IronmanClient" : reqContext.Race.ApiName.ToLower();
            if (apiName.Equals("ironmanclientdoubletable"))
            {
                sourceClient = new IronmanClientDoubleTable(_DBContext);
            }
            else
            {
                sourceClient = new IronmanClient(_DBContext);
            }
            int pagenum = 1;
                
            List<Triathlete> athletesPerPage = new List<Triathlete>();


            IRestResponse response = sourceClient.MakeRequest(baseUrl, sourceClient.BuildRequestParameters(pagenum, reqContext));
            bool result = sourceClient.HandleResponse(response, reqContext);
            if (result)
            {
                sourceClient.ParseDataToVerify(reqContext,response.Content);

                SaveRequestContext(reqContext);

            }
        }
        

        public void VerifyRace(Race race)
        {
          var query = _DBContext.RequestContext.Where(r => r.RaceId == race.RaceId);
          foreach(RequestContext req in query)
          {
                VerifyRequestContext(req);
          }
        }

        /***********************************************************
         * GetAthletesByAgeGroup(race,gender)
         * retrieve athletes of race with given gender (male and female)
         *************************************************************/
        public List<Triathlete> GetAthletesByAgeGroup(string raceId, int agegroupId)
        {

            var query = _DBContext.Triathletes
                              .Where(t => t.RequestContext.RaceId == raceId)
                              .Where(t => t.RequestContext.AgeGroupId == agegroupId);

            return query.ToList();

        }
        public List<Triathlete> GetAthletesByGender(string raceId, int genderId)
        {

            var query = _DBContext.Triathletes
                              .Where(t => t.RequestContext.RaceId == raceId)
                              .Where(t => t.RequestContext.GenderId == genderId);

            return query.ToList();

        }
        public List<Triathlete> GetAthletesByRace(string raceId)
        {

            int[] ageGroupIds = _DBContext.AgeGroups.Select(n => n.AgeGroupId).ToArray();

            var query = _DBContext.Triathletes
                             .Where(t => t.RequestContext.RaceId == raceId)
                             .Where(t => ageGroupIds.Contains(t.RequestContext.AgeGroupId));

            return query.ToList();

        }


        public List<Triathlete> GetAthletesByName_UseElasticSearch(string name)
        {
            var search = new ElasticSearchFacade(_DBContext);
            return search.SearchAthletesFieldQuery("name", name);
        }

       
       
        public List<Triathlete> GetAthletesByName(string name, string[] raceIds = null)
        {
            var formattedName = Triathlete.FormatName(name);
            var query = _DBContext.Triathletes
                                  .Where(a => a.Name.Contains(formattedName));
            if (raceIds != null)
                query = query.Where(t => raceIds.Contains(t.RequestContext.RaceId));

           
            return query.ToList();
        }
        public Triathlete GetAthleteById(int id)
        {
            var athlete = _DBContext.Triathletes.Where(a => a.TriathleteId == id);
            return athlete.SingleOrDefault();
        }

  
        public List<Race> GetRacesByGroupName(string name)
        {

            IQueryable<Race> query = _DBContext.Races;
            if (!String.IsNullOrEmpty(name) && !name.Equals("0"))
                query = query.Where(a => a.ShortName.Equals(name));

            return query.ToList();
        }

        public List<Race> GetRacesByTagId(List<int> tagIds)
        {
            var races = _DBContext.Races.Where(r => r.Conditions.RaceConditionTags.Select(t => t.TagId).Intersect(tagIds).Any());

            return races.ToList();
        }

        public List<Race> GetRacesById(string id)
        {
            IQueryable<Race> query = _DBContext.Races; 

            if (!id.Equals("0"))
            {
                query = query.Where(r => r.RaceId == id);
            }

            return query.ToList();
        }

        public List<Race> GetRacesBySwimCondition(string conditions)
        {
            var search = new ElasticSearchFacade(_DBContext);
            return search.SearchRaceConditions("conditions.swim*.tag.value", conditions);
        }
        public List<Race> GetRacesByBikeCondition(string conditions)
        {
            var search = new ElasticSearchFacade(_DBContext);
            return search.SearchRaceConditions("conditions.bike*.tag.value", conditions);
        }
        public List<Race> GetRacesByRunCondition(string conditions)
        {
            var search = new ElasticSearchFacade(_DBContext);
            return search.SearchRaceConditions("conditions.run*.tag.value", conditions);
        }

        public void ReIndex()
        {
            var search = new ElasticSearchFacade(_DBContext);
            search.ReIndexRaces();
            search.ReIndexTriathletes();
        }

#region Private Methods
        
        private RequestContext CreateRequestContext(string raceId, int agegroupId, int genderId)
        {
            RequestContext req = new RequestContext();
            req.RaceId = raceId;
            req.Race = _DBContext.Races.Single(a => a.RaceId == raceId);

            req.AgeGroupId = _DBContext.AgeGroups.Single(a => a.AgeGroupId == agegroupId).AgeGroupId;
            req.AgeGroup = _DBContext.AgeGroups.Single(a => a.AgeGroupId == agegroupId);

            req.GenderId = _DBContext.Genders.Single(g => g.GenderId == genderId).GenderId;
            req.Gender = _DBContext.Genders.Single(g => g.GenderId == genderId);



            return req;
        }


        private RequestContext GetRequestContext(string raceId, int agegroupId, int genderId)
        {
            Trace.TraceInformation(String.Format("GetRequestContext {0}-{1}-{2}", raceId, agegroupId, genderId));
     
            RequestContext req = _DBContext.RequestContext.SingleOrDefault(i => i.RaceId == raceId &&
                                i.AgeGroupId == agegroupId && i.GenderId == genderId);

            return req;  //NOTE: THIS will return null if context not found
        }
        private List<Triathlete> GetAthletesFromDb(RequestContext req)
        {
            List<Triathlete> athletes;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var query = from t in _DBContext.Triathletes
                        where t.RequestContextId == req.RequestContextId
                        select t;

            athletes = query.ToList();

            stopwatch.Stop();

   //         CacheService.Instance.PopulateAthletes(req, athletes);

            Trace.TraceInformation(String.Format("GetAthletesFromStorage took: {0} ", stopwatch.Elapsed.ToString()));

            return athletes;
        }
        private List<Triathlete> GetAthletesFromCacheTestXX(RequestContext req)
        {
            List<Triathlete> athletes;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            athletes = CacheService.Instance.GetAthletes(req);
            stopwatch.Stop();

            CacheService.Instance.PopulateAthletes(req, athletes);

     //       Trace.TraceInformation(String.Format("GetAthletesFromCache took: {0} ", stopwatch.Elapsed.ToString()));

            return athletes;
        }



        private List<Triathlete> GetAthletesFromSource(RequestContext reqContext)
        {

            Trace.TraceInformation(String.Format("GetFromSource {0}-{1}{2}", reqContext.RaceId, reqContext.AgeGroupId, reqContext.GenderId));
                 
            List<Triathlete> athletesFromSource = new List<Triathlete>();
            reqContext.SourceCount = 0;

            string baseUrl = reqContext.Race.BaseURL;

            IronmanClient sourceClient;  //we'll change this to a factory pattern if we have more than these two classes

            string apiName = reqContext.Race.ApiName == null ? "IronmanClient" : reqContext.Race.ApiName.ToLower();
            if (apiName.Equals("ironmanclientdoubletable"))
            {
                sourceClient = new IronmanClientDoubleTable(_DBContext);
            }
            else
            {
                sourceClient = new IronmanClient(_DBContext);
            }
            for (int pagenum = 1; pagenum < 100; pagenum++)
            {
                List<Triathlete> athletesPerPage = new List<Triathlete>();


                IRestResponse response = sourceClient.MakeRequest(baseUrl, sourceClient.BuildRequestParameters(pagenum, reqContext));
                bool result = sourceClient.HandleResponse(response, reqContext);
                if (result)
                {
                    athletesPerPage = sourceClient.ParseData(reqContext, response.Content,pagenum);

                    if (athletesPerPage.Count > 0)
                    {
                        reqContext.SourceCount += athletesPerPage.Count;
                        athletesFromSource.AddRange(athletesPerPage);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

   
            SaveRequestContext(reqContext);
            DeleteAthletes(reqContext);
            SaveAthletes(athletesFromSource);



            return athletesFromSource;
        }



        private void DeleteAthletes(RequestContext reqContext)
        {
            var range = _DBContext.Triathletes.Where(x => x.RequestContextId == reqContext.RequestContextId);
            var removed = _DBContext.Triathletes.RemoveRange(range);
            var test = removed.Count();
            _DBContext.SaveChanges();

        }

        private void SaveRequestContext(RequestContext req)
        {
            if (req.Status == "OK")
            {
                req.Instruction = RequestInstruction.Normal;
                req.LastRequestedUTC = DateTime.Now.ToUniversalTime();
                _DBContext.RequestContext.AddOrUpdate(req);
                _DBContext.SaveChanges();
            }
            else //go ahead and save this; it could be that there were no athletes in this age division.
            {

                req.Status = String.Format("{0} A:{1} G:{2} : {3} ", req.Race.DisplayName, req.AgeGroup.DisplayName, req.Gender.DisplayName, req.Status);
                req.Instruction = RequestInstruction.Normal;
                req.LastRequestedUTC = DateTime.Now.ToUniversalTime();



                _DBContext.RequestContext.AddOrUpdate(req);
                _DBContext.SaveChanges();

            }

        }

        private void SaveAthletes(List<Triathlete> athletes)
        {
            //running into error using AddOrUpdate, so trying to do this iteravely   
            List<Triathlete> athletesToSave = new List<Triathlete>();
            foreach (var entity in athletes)
            {
                var entityinDB = _DBContext.Triathletes.Find(entity.TriathleteId);
                if (entityinDB == null)
                    athletesToSave.Add(entity);
            }


            // var entityinDB = _DBContext.Triathletes.Where(t => t.Name == "Randall, Wild Bill");
            //  var entinSave = athletesToSave.Where(t => t.Name == "Randall, Wild Bill");

            _DBContext.Triathletes.AddOrUpdate(athletesToSave.ToArray());

            _DBContext.SaveChanges();

        }

        

        #endregion //Private

    }
}



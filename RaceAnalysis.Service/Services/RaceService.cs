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
        public List<Triathlete> GetAthletes(IRaceCriteria criteria)
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
                        if (reqContext == null || reqContext.Instruction == RequestInstruction.ForceSource)
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
                            athletesInReqContext = GetAthletesFromStorage(reqContext);
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
                            athletesInReqContext = GetAthletesFromStorage(reqContext);
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

        //For the near-term we are going directly to database rather than use elastic search to ease deployment
        public List<Triathlete> GetAthletesByName(string name)
        {
            var athletes = _DBContext.Triathletes.Where(a => a.Name.Contains(name));
            return athletes.ToList();
        }

        public List<Race> GetRacesByTagId(List<int> tagIds)
        {
            var races = _DBContext.Races.Where(r => r.Conditions.RaceConditionTags.Select(t => t.TagId).Intersect(tagIds).Any());

            return races.ToList();
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
            RequestContext req = _DBContext.RequestContext.SingleOrDefault(i => i.RaceId == raceId &&
                                i.AgeGroupId == agegroupId && i.GenderId == genderId);

            return req;  //NOTE: THIS will return null if context not found
        }
        private List<Triathlete> GetAthletesFromStorage(RequestContext req)
        {
            List<Triathlete> athletesInKeyContext;

            var query = from t in _DBContext.Triathletes
                        where t.RequestContextId == req.RequestContextId
                        select t;

            athletesInKeyContext = query.ToList();

            return athletesInKeyContext;
        }



        private List<Triathlete> GetAthletesFromCacheDyanamic(RequestContext reqContext)
        {
            FilterRule outerFilter = new FilterRule();
            outerFilter.Condition = "and";
            outerFilter.Rules = new List<FilterRule>();

            if (!String.IsNullOrEmpty(reqContext.RaceId))
            {
                FilterRule raceFilter = new FilterRule();
                raceFilter.Field = "RaceId";
                raceFilter.Operator = "equal";
                raceFilter.Value = reqContext.RaceId;
                raceFilter.Type = "integer";
                outerFilter.Rules.Add(raceFilter);
            }

            if (reqContext.AgeGroupId != 0)
            {
                FilterRule agegroupFilter = new FilterRule();
                agegroupFilter.Field = "AgeGroupId";
                agegroupFilter.Operator = "equal";
                agegroupFilter.Value = reqContext.AgeGroupId.ToString();
                agegroupFilter.Type = "integer";
                outerFilter.Rules.Add(agegroupFilter);
            }
            if (reqContext.GenderId != 0)
            {
                FilterRule genderFilter = new FilterRule();
                genderFilter.Field = "GenderId";
                genderFilter.Operator = "equal";
                genderFilter.Value = reqContext.GenderId.ToString();
                genderFilter.Type = "integer";
                outerFilter.Rules.Add(genderFilter);
            }

            List<int> contextIds = _DBContext.RequestContext.BuildQuery(outerFilter).Select(c => c.RequestContextId).ToList();

            var query = _DBContext.Triathletes
                             .Where(t => contextIds.Contains(t.RequestContextId));

            List<Triathlete> filteredCollection = query.ToList();


            return filteredCollection;
        }

        private List<Triathlete> GetAthletesFromCacheDyanamic(int[] raceIds, int[] agegroupIds, int[] genderIds)
        {
            FilterRule outerFilter = new FilterRule();
            outerFilter.Condition = "and";
            outerFilter.Rules = new List<FilterRule>();

            if (raceIds != null)
            {
                FilterRule raceFilter = new FilterRule();
                raceFilter.Field = "RaceId";
                raceFilter.Condition = "or";
                raceFilter.Operator = "in";
                raceFilter.Value = String.Join(",", raceIds.Select(p => p.ToString()).ToArray());
                raceFilter.Type = "integer";
                outerFilter.Rules.Add(raceFilter);
            }

            if (agegroupIds != null)
            {
                FilterRule agegroupFilter = new FilterRule();
                agegroupFilter.Field = "AgeGroupId";
                agegroupFilter.Condition = "or";
                agegroupFilter.Operator = "in";
                agegroupFilter.Value = String.Join(",", agegroupIds.Select(p => p.ToString()).ToArray());
                agegroupFilter.Type = "integer";
                outerFilter.Rules.Add(agegroupFilter);
            }
            if (genderIds != null)
            {
                FilterRule genderFilter = new FilterRule();
                genderFilter.Field = "GenderId";
                genderFilter.Condition = "or";
                genderFilter.Operator = "in";
                genderFilter.Value = String.Join(",", genderIds.Select(p => p.ToString()).ToArray());
                genderFilter.Type = "integer";
                outerFilter.Rules.Add(genderFilter);
            }

            List<int> contextIds = _DBContext.RequestContext.BuildQuery(outerFilter).Select(c => c.RequestContextId).ToList();

            var query = _DBContext.Triathletes
                             .Where(t => contextIds.Contains(t.RequestContextId));

            List<Triathlete> filteredCollection = query.ToList();


            return filteredCollection;
        }


        private List<Triathlete> GetAthletesFromSource(RequestContext reqContext)
        {
            List<Triathlete> athletesFromSource = new List<Triathlete>();

            string baseUrl = reqContext.Race.BaseURL;

            IronmanClient client = new IronmanClient(_DBContext);

            for (int pagenum = 1; pagenum < 100; pagenum++)
            {
                List<Triathlete> athletesPerPage = new List<Triathlete>();


                IRestResponse response = client.MakeRequest(baseUrl, client.BuildRequestParameters(pagenum, reqContext));
                bool result = client.HandleResponse(response, reqContext);
                if (result)
                {
                    athletesPerPage = IronmanClient.ParseData(reqContext, response.Content);

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

            //changed to accumulate   reqContext.SourceCount = athletesFromSource.Count;

            SaveRequestContext(reqContext);
            SaveAthletes(athletesFromSource);



            return athletesFromSource;
        }

        private void DeleteAthletes(int reqContextId)
        {
            var range = _DBContext.Triathletes.Where(x => x.RequestContextId == reqContextId);
            var removed = _DBContext.Triathletes.RemoveRange(range);
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


        /*************
        private List<Triathlete> GetAthletesFromSource_SAVE-MAY NEED THIS LATER(int[] raceIds, int[] agegroupIds, int[] genderIds)
        {
            List<Triathlete> athletesFromSource = new List<Triathlete>();

            //create a reqContext for each combination. 
            foreach (int raceId in raceIds) 
            {
                foreach (int ageId in agegroupIds)
                {
                    foreach (int genderId in genderIds)
                    {
                        RequestContext reqContext = GetRequestContext(raceId, ageId, genderId);

                        string baseUrl = reqContext.Race.BaseURL;


                        RestClientX client = new RestClientX();
                        for (int pagenum = 1; pagenum < 100; pagenum++)
                        {

                            string response = client.MakeRequest(baseUrl, BuildRequestParameters(pagenum,ageId,genderId));

                            List<Triathlete> athletesPerPage = Triathlete.ParseData(reqContext, response);
                            if (athletesPerPage.Count > 0)
                            {
                                athletesFromSource.AddRange(athletesPerPage);
                            }
                            else
                            {
                                break;
                            }
                        }


                        //TO-DO: some of the contexts result in zero athletes, and we may not want to go back to the 
                        //source since it will result in zero. Need to figure out how to differentiate the empy result
                        //set from the neveer-been-checked cache
                        _DBContext.RequestContext.Add(reqContext);
                        _DBContext.SaveChanges();

                        _DBContext.Triathletes.AddRange(athletesFromSource);
                        _DBContext.SaveChanges();
                    }
                }
            }
            return athletesFromSource;
        } 
        ****************/


        #endregion //Private

    }
}



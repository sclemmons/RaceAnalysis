using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RaceAnalysis.Helpers;
using RaceAnalysis.Models;
using System.Linq.Dynamic;
using Castle.DynamicLinqQueryBuilder;
using System.Data.Entity.Migrations;
using RestSharp;
using X.PagedList;

namespace RaceAnalysis.Controllers
{
    public abstract class BaseController : Controller
    {
        protected RaceAnalysisDbContext _DBContext = new RaceAnalysisDbContext();
        public BaseController()
        {
           
        }

        
        public ActionResult ShowFilterByRace(string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel(_DBContext);
            viewmodel.SaveRaceFilterValues(races,agegroups,genders);

            return PartialView("~/Views/Shared/_RaceFilter.cshtml", viewmodel);
        }
        public ActionResult ShowFilterByDuration(string races, string agegroups, string genders)
        {
            var viewmodel = new TriStatsViewModel();
            viewmodel.Filter = new RaceFilterViewModel(_DBContext);
            viewmodel.Filter.SaveRaceFilterValues(races, agegroups, genders);

            return PartialView("~/Views/Shared/_SearchByDuration.cshtml", viewmodel);
        }

        [HttpPost]
        public ActionResult SearchByAthleteName(FormCollection form, string races, string agegroups, string genders)
        {
            return SearchOpenFieldQuery("name", form["SearchString"], races, agegroups, genders);
        }


        [HttpPost]
        public ActionResult SearchByTimeThresholds(FormCollection form, string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel(_DBContext);
            viewmodel.SaveRaceFilterValues(races, agegroups, genders);

            var swimMinsLow = Convert.ToInt16(form["swim-lowtime-value"].ZeroIfEmpty());
            var swimLow = new TimeSpan(0, swimMinsLow, 0);

            var swimMinsHigh = Convert.ToInt16(form["swim-hightime-value"].MaxIfEmpty());
            var swimHigh = new TimeSpan(0, swimMinsHigh, 0);

            var bikeMinsLow = Convert.ToInt16(form["bike-lowtime-value"].ZeroIfEmpty());
            var bikeLow = new TimeSpan(0, bikeMinsLow, 0);

            var bikeMinsHigh = Convert.ToInt16(form["bike-hightime-value"].MaxIfEmpty());
            var bikeHigh = new TimeSpan(0, bikeMinsHigh, 0);
            
            var runMinsLow = Convert.ToInt16(form["run-lowtime-value"].ZeroIfEmpty());
            var runLow = new TimeSpan(0, runMinsLow, 0);

            var runMinsHigh = Convert.ToInt16(form["run-hightime-value"].MaxIfEmpty());
            var runHigh = new TimeSpan(0, runMinsHigh, 0);

            var finishMinsLow = Convert.ToInt16(form["finish-lowtime-value"].ZeroIfEmpty());
            var finishLow = new TimeSpan(0, finishMinsLow, 0);

            var finishMinsHigh = Convert.ToInt16(form["finish-hightime-value"].MaxIfEmpty());
            var finishHigh = new TimeSpan(0, finishMinsHigh, 0);


            ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);

            var athletes = search.SearchByDuration(swimLow, swimHigh, bikeLow, bikeHigh, runLow, runHigh, finishLow,finishHigh,viewmodel);
            return DisplayResultsView(athletes,viewmodel);
        }
        private ActionResult SearchOpenFieldQuery(string field, string query, string races, string agegroups, string genders)
        {
            var viewmodel = new RaceFilterViewModel(_DBContext);
            viewmodel.SaveRaceFilterValues(races, agegroups, genders);

            ElasticSearchFacade search = new ElasticSearchFacade(_DBContext);

            var athletes = search.SearchFieldQuery(field, query);

            return DisplayResultsView(athletes,viewmodel);
        }

        //called from the racefilter
        public ActionResult SelectedRaces(PostedFilterValues postedValues)
        {
            var filter = new RaceFilterViewModel(_DBContext);
            filter.SaveRaceFilterValues(postedValues);

            int[] raceIds = postedValues.RaceIds != null ?
                Array.ConvertAll(postedValues.RaceIds, element => Convert.ToInt32(element)) :
                 new int[] { };


            int[] agegroupIds = postedValues.AgeGroupIds != null ?
                Array.ConvertAll(postedValues.AgeGroupIds, element => Convert.ToInt32(element)) :
                new int[] { };


            int[] genderIds = postedValues.GenderIds != null ?
                Array.ConvertAll(postedValues.GenderIds, element => Convert.ToInt32(element)) :
                new int[] { };


            return DisplayResultsView( 1, raceIds, agegroupIds, genderIds);

        }
        //called from the Paging Control
        //this is the best way I found to pass the arrays from view to controller
        public ActionResult DisplayPagedAthletes(int page, string races, string agegroups, string genders)
        {
            var r = Array.ConvertAll(races.Split(','), int.Parse);
            var a = Array.ConvertAll(agegroups.Split(','), int.Parse);
            var g = Array.ConvertAll(genders.Split(','), int.Parse);

            return DisplayResultsView(page, r, a, g);
        }



        #region Protected Methods
        protected abstract ActionResult DisplayResultsView(int page, int[] races, int[] agegroups, int[] genders);

        protected virtual ActionResult DisplayResultsView(List<Triathlete> athletes, RaceFilterViewModel filter)
        {
            var viewmodel = new TriathletesViewModel();
            viewmodel.Filter = filter;
            viewmodel.Triathletes = athletes.ToPagedList(pageNumber: 1, pageSize: 100); //max xx per page

            return View("List", viewmodel);
        }

        protected bool GetTrueOrFalseFromCheckbox(string val)
        {
            string[] values = val.Split(',');
            if (values != null && values.Length > 1)
                return true;
            else
                return false;

        }


        protected RequestContext GetRequestContextFromCache(int raceId, int agegroupId, int genderId)
        {
            RequestContext req = _DBContext.RequestContext.SingleOrDefault(i => i.RaceId == raceId &&
                                i.AgeGroupId == agegroupId && i.GenderId == genderId);
            
            return req;  //NOTE: THIS will return null if context not found
        }

        protected RequestContext CreateRequestContext(int raceId, int agegroupId, int genderId)
        {
                RequestContext req = new RequestContext();
                req.RaceId = raceId;
                req.Race = _DBContext.Races.Single(a => a.RaceId == raceId);

                req.AgeGroupId = _DBContext.AgeGroups.Single(a => a.AgeGroupId == agegroupId).AgeGroupId;
                req.AgeGroup = _DBContext.AgeGroups.Single(a => a.AgeGroupId == agegroupId);

                req.GenderId = _DBContext.Genders.Single(g => g.GenderId == genderId).GenderId;
                req.Gender  = _DBContext.Genders.Single(g => g.GenderId == genderId);



            return req;
        }



        /***************************************
         * GetAthletes()
         * Retrieve the athletes with the given request values
         * ****************************************/
        protected List<Triathlete> GetAthletes(int[] raceIds,int[] agegroupIds,int[] genderIds)
        {

            List<Triathlete> allAthletes = new List<Triathlete>();

            //create a requestContext for each combination. 
            foreach (int raceId in raceIds)
            {
                foreach (int ageId in agegroupIds)
                {
                    foreach (int genderId in genderIds)
                    {
                        RequestContext reqContext = GetRequestContextFromCache(raceId, ageId, genderId);
                        List<Triathlete> athletesInReqContext = new List<Triathlete>();
                        if (reqContext == null || reqContext.Instruction == RequestInstruction.ForceSource)
                        {
                            if (reqContext == null)
                                reqContext = CreateRequestContext(raceId, ageId, genderId);
                            else
                            { //we need to remove any athletes that might be there under that context to avoid dupes
                                DeleteAthletes(reqContext.RequestContextId);
                            }

                            //get from source 
                            athletesInReqContext = GetAthletesFromSource(reqContext);
                        }
                        else
                        {
                             athletesInReqContext = GetAthletesFromCache(reqContext);
                        }

                        if (athletesInReqContext.Count > 0)
                        {
                            allAthletes.AddRange(athletesInReqContext);
                        }
                    }

                }
            }
 
            return allAthletes;
        }

        #endregion //Protected Methods

        #region Private Methods
        private void DeleteAthletes(int reqContextId)
        {
            var range = _DBContext.Triathletes.Where(x => x.RequestContextId == reqContextId);
            var removed = _DBContext.Triathletes.RemoveRange(range);
            _DBContext.SaveChanges();
           
        }
   
        /***********************************************************
         * GetAthletesByAgeGroup(race,gender)
         * retrieve athletes of race with given gender (male and female)
         *************************************************************/ 
        private List<Triathlete> GetAthletesByAgeGroup(int raceId, int agegroupId)
        {

            var query = _DBContext.Triathletes
                              .Where(t => t.RequestContext.RaceId == raceId)
                              .Where(t => t.RequestContext.AgeGroupId == agegroupId);

            return query.ToList();

        }
        private List<Triathlete> GetAthletesByGender(int raceId,int genderId)
        {

           var query = _DBContext.Triathletes
                             .Where (t => t.RequestContext.RaceId == raceId )
                             .Where(t => t.RequestContext.GenderId == genderId);

            return query.ToList();

        }
        private List<Triathlete> GetAthletesByRace(int raceId)
        {

            int[] ageGroupIds = _DBContext.AgeGroups.Select(n => n.AgeGroupId).ToArray();

            var query = _DBContext.Triathletes
                             .Where(t => t.RequestContext.RaceId == raceId)
                             .Where(t => ageGroupIds.Contains(t.RequestContext.AgeGroupId));

            return query.ToList();

        }

        private List<Triathlete> GetAthletesFromCache(RequestContext req)
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

            if (reqContext.RaceId != 0)
            {
                FilterRule raceFilter = new FilterRule();
                raceFilter.Field = "RaceId";
                raceFilter.Operator = "equal";
                raceFilter.Value = reqContext.RaceId.ToString();
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

            List<int>contextIds = _DBContext.RequestContext.BuildQuery(outerFilter).Select(c => c.RequestContextId).ToList();

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
                bool result = client.HandleResponse(response,reqContext);
                if (result)
                {
                    athletesPerPage = IronmanClient.ParseData(reqContext, response.Content);

                    if (athletesPerPage.Count > 0)
                    {
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

            reqContext.SourceCount = athletesFromSource.Count;
                 
            SaveRequestContext(reqContext);
            SaveAthletes(athletesFromSource);
            
           
            
            return athletesFromSource;
        }
         private void SaveRequestContext(RequestContext req)
        {
            if (req.Status == "OK")
            {
                req.Instruction = RequestInstruction.Normal; // force the request next time
                req.LastRequestedUTC = DateTime.Now.ToUniversalTime();
                _DBContext.RequestContext.AddOrUpdate(req);
                _DBContext.SaveChanges();
            }
        
        }
        private void SaveAthletes (List<Triathlete> athletesToSave)
        {
            /***
           List<Triathlete> athletesToSave = new List<Triathlete>();
           foreach (var entity in athletesFromSource)
           {
               var entityinDB = _DBContext.Triathletes.Find(entity.Name, entity.RequestContextId);
               if (entityinDB == null)
                   athletesToSave.Add(entity);
           }
           ***/
         
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
                        RequestContext reqContext = GetRequestContextFromCache(raceId, ageId, genderId);

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


        #endregion//Private Methods

    }
}
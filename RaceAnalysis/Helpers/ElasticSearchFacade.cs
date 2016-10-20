using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;
using Elasticsearch.Net;
using RaceAnalysis.Models;
using System.Diagnostics;
using System.Text;

namespace RaceAnalysis.Helpers
{
    public class ElasticSearchFacade
    {
        protected RaceAnalysisDbContext _DBContext;
        public ElasticSearchFacade(RaceAnalysisDbContext dbCtx)
        {
            _DBContext = dbCtx;
        }

        public KeyValueViewModel SearchCountPerCountry()
        {
            var client = SetupElasticSearch();
            var request = new SearchRequest<Triathlete>
            {
                Aggregations = new TermsAggregation("countries")
                {
                    Field = new Field { }.Name = "country",
                    MinimumDocumentCount = 0,
                    Size = 0,
                    Missing = "n/a",
                    Order = new List<TermsOrder>
                  {
                      TermsOrder.CountDescending,
                      TermsOrder.TermAscending
                  }


                }

            };

            var response = client.Search<Triathlete>(request);
            var countries = response.Aggs.Terms("countries");

            var model = new KeyValueViewModel();
            model.KeyValuePairs = new List<KeyValuePair<string, string>>();
            foreach (var item in countries.Buckets)
            {
                model.KeyValuePairs.Add(
                    new KeyValuePair<string, string>(
                        item.Key.ToUpper(),
                        item.DocCount.ToString())
                    );
            }

            return model;
         
        }

    

        public List<Triathlete> SearchByDuration(TimeSpan swimL,TimeSpan swimH, 
                                                    TimeSpan bikeL, TimeSpan bikeH,
                                                      TimeSpan runL, TimeSpan runH,
                                                        TimeSpan finishL,TimeSpan finishH,
                                                            RaceFilterViewModel filter)
        {

            var reqIds = filter.GetRequestIds(filter);
            QueryContainer orQuery = null;

            foreach (var id in reqIds)
            {
                orQuery |= new TermQuery
                {
                    Field = "requestContextId",  //case sensitive!! found out the hard way
                    Value = id
                };
            }

            var client = SetupElasticSearch();

            var response = client.Search<Triathlete>(s => s
              .Query(q =>
                   q.Bool(b => b.Must(orQuery))
               &&
                   q.Range(c => c
                      .Name("swim_query")
                          .Field("swim")
                              .GreaterThan(swimL.Ticks)
                              .LessThan(swimH.Ticks)) 
                 &&
                   q.Range(c => c
                      .Name("bike_query")
                          .Field("bike")
                              .GreaterThan(bikeL.Ticks)
                              .LessThan(bikeH.Ticks))

                 &&
                   q.Range(c => c
                      .Name("run_query")
                          .Field("run")
                              .GreaterThan(runL.Ticks)
                              .LessThan(runH.Ticks))

                 &&
                   q.Range(c => c
                      .Name("finish_query")
                          .Field("finish")
                              .GreaterThan(finishL.Ticks)
                              .LessThan(finishH.Ticks))
               ));
            
            /*****
            var response = client.Search<Triathlete>(s => s
                .Query(q => q
                    .Range(c => c
                       .Name("swim_query")
                           .Field("swim")
                               .LessThan(swim.Ticks))

                )); 
                ****/
              
             var athletes = response.Documents.ToList();

            return athletes;
        }
        public List<Triathlete> SearchFieldQuery(string field, string queryValue)
        {
            var client = SetupElasticSearch();
            var request = new SearchRequest
            {
                From = 0,
                Size = 100,
                //Query = new MatchQuery { Field = "name", Query = "clemmons" }
                Query = new MatchQuery { Field = field, Query = queryValue }

            };

            var response = client.Search<Triathlete>(request);

            var athletes = response.Documents.ToList();

            return athletes;

          
        }
    

    
        const string myindex = "mytriindex";
        /*** The following version demonstrates a few techniques when indexing****/
        public void ReIndex()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                        .DefaultIndex(myindex)
                        .MapPropertiesFor<Race>(m => m.Ignore(r => r.BaseURL))
                        .MapPropertiesFor<RequestContext>(m => m
                          .Ignore(v => v.Instruction));      //Leaves out this field entire in mapping and _source so it doesn't get serialized
                                                             //Does the same as jsonIgnore attribute on the field
                                                             //but this does it in a more dynamic manner so we can
                                                             //include the field on specific indexes.
            var client = new ElasticClient(settings);
            if (client.IndexExists(myindex).Exists)
                client.DeleteIndex(myindex);

            client.CreateIndex(myindex, i => i
                .Settings(s => s
                    .NumberOfShards(2)
                    .NumberOfReplicas(0)
           )
            .Mappings(ms => ms
                .Map<Triathlete>(m => m
                    .AutoMap()
                    .Properties(ps => ps
                        .Nested<RequestContext>(n => n
                        .Name(c => c.RequestContext)
                        .Fields(fs => fs
                            .String(s => s
                               .Name("status")
                               .Index(FieldIndexOption.NotAnalyzed)    //the status field still remains in the source
                                ))

                        .AutoMap()
                                .Properties(rcp => rcp
                                    .Nested<Race>(r => r
                                    .Name(d => d.Race)
                                    .AutoMap()
                                           .Properties(rp => rp
                                                .Nested<RaceConditions>(rcn => rcn
                                                .Name(rcnn => rcnn.Conditions)
                                                .Enabled(false)         //I cannot tell at this point what this does, but all of the conditions still get into the source
                                                .IncludeInParent(false)
                                                .AutoMap()
                                                )))))))));

            //bulk index:
            var res = client.IndexMany<Triathlete>(_DBContext.Triathletes);

            /*****
            int count = 0;
            foreach (var t in _DBContext.Triathletes
                .Include("RequestContext")
                .Include("RequestContext.Race")
                .Include("RequestContext.Gender")
                .Include("RequestContext.AgeGroup")
               )
            {
                RequestContext r = t.RequestContext;
                Race ra = t.Race;

                count++;

       

                if (count > 10) break;
            }
            ******/

        }


        //This version includes the race information that goes with the triathlete
        //it requires that the Triathlete.Race be available for serialization and all of the nested classes.
        public void ReIndexObsolete()
        {
            var client = SetupElasticSearch();

            var descriptor = new CreateIndexDescriptor("mytriindex")
            .Mappings(ms => ms
                .Map<Triathlete>(m => m.AutoMap())
                .Map<RequestContext>(m => m.AutoMap())
            );


            int count = 0;
            foreach (var t in _DBContext.Triathletes
                .Include("RequestContext")
                .Include("RequestContext.Race")
                .Include("RequestContext.Gender")
                .Include("RequestContext.AgeGroup")
                .Include("RequestContext.Race.Conditions")
               )
            {
                RequestContext r = t.RequestContext;
                Race ra = t.Race;

                count++;
                client.Index(t, idx => idx.Index("mytriindex"));

                if (count > 10) break;
            }


           
        }




        private ElasticClient SetupElasticSearch()
        {

            var nodes = new Uri[]
            {
                 new Uri("http://localhost:9200")
            //    new Uri("http://ipv4.fiddler:9200")  //use fiddler to see messages to & from elasticsearch
            };
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
/**** 
 * USE THE FOLLOWING CODE to DEBUG or LOG without Fiddler 
            var list = new List<string>();
     
            settings
            .DisableDirectStreaming()
                .DisableAutomaticProxyDetection(true)
                    .OnRequestCompleted(response =>
                    {
                        // log out the request and the request body, if one exists for the type of request
                        if (response.RequestBodyInBytes != null)
                        {
                            list.Add(
                                $"{response.HttpMethod} {response.Uri} \n" +
                                $"{Encoding.UTF8.GetString(response.RequestBodyInBytes)}"); //this is where the request JSON will go
                        }
                        else
                        {
                            list.Add($"{response.HttpMethod} {response.Uri}");
                        }

                        // log out the response and the response body, if one exists for the type of response
                        if (response.ResponseBodyInBytes != null)
                        {
                            list.Add($"Status: {response.HttpStatusCode}\n" +
                                     $"{Encoding.UTF8.GetString(response.ResponseBodyInBytes)}\n" +
                                     $"{new string('-', 30)}\n");
                        }
                        else
                        {
                            list.Add($"Status: {response.HttpStatusCode}\n" +
                                     $"{new string('-', 30)}\n");
                        }
                    });
                    ****/

            var client = new ElasticClient(settings);
            return client;
        }
           
    }
}
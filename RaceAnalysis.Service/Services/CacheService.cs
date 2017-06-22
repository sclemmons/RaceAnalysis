using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Diagnostics;
using RaceAnalysis.Models;
using RaceAnalysis.Service.Interfaces;

namespace RaceAnalysis.Service
{
    public class CacheService : ICacheService
    {
        private const string _CacheKeyShallowAthletes = "ShallowAthletes";

        private static readonly CacheService _Instance = new CacheService();
        public static CacheService Instance
        {
            get
            {
                return _Instance;
            }
        }


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CacheService()
        {

        }

        //making this public for Unity Injection
        public CacheService() { }


        public void PopulateCache(List<ShallowTriathlete> athletes)
        {
            //  HttpContext.Current.Application.Lock();
            //            Trace.TraceInformation("PopulateShallowAth");
            IDatabase cache = Connection.GetDatabase();
        
            cache.StringSet(_CacheKeyShallowAthletes, JsonConvert.SerializeObject(athletes));

            //   HttpContext.Current.Application.UnLock();
        }


        public List<ShallowTriathlete> GetShallowAthletes()
        {
            //Trace.TraceInformation("GetShallowathlete");

            List<ShallowTriathlete> athletes = null;
            IDatabase cache;

            //   try
            {
                cache = Connection.GetDatabase();
                string serializedAthletes = cache.StringGet(_CacheKeyShallowAthletes);

                if (!String.IsNullOrEmpty(serializedAthletes))
                {
                    athletes = JsonConvert.DeserializeObject<List<ShallowTriathlete>>(serializedAthletes);
                }

            }
            /**
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);

            }
            **/
        
            return athletes == null ? new List<ShallowTriathlete>() : athletes;
        }

        public void FlushShallowAthletes()
        {
            IDatabase cache = Connection.GetDatabase();
            cache.KeyDelete(_CacheKeyShallowAthletes);
           
        }
      

        private static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }


        // Redis Connection string info
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"];
            //   Trace.TraceInformation("connection: " + cacheConnection);
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        

    }
}

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
using System.Threading.Tasks;

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


        public async Task PopulateCacheAsync(List<ShallowTriathlete> athletes)
        {
            //  HttpContext.Current.Application.Lock();
            Trace.TraceInformation("PopulateShallowAth");
            IDatabase cache = Connection.GetDatabase();

      //      cache.KeyDelete(_CacheKeyShallowAthletes);

           

            var tasks = new List<Task>();
            double i = 0;
            foreach (var a in athletes)
            {
                string value = string.Format("{0}#{1}#{2}", a.Name.ToLower(), a.Id, a.RaceId.ToUpper());
                tasks.Add(cache.SortedSetAddAsync(_CacheKeyShallowAthletes, value,i++, CommandFlags.FireAndForget));

            }

            await Task.WhenAll(tasks);

            Trace.TraceInformation("PopulatEShallow-End");


            //   HttpContext.Current.Application.UnLock();
        }

       
        public List<ShallowTriathlete> GetShallowAthletes(string name)
        {
            Trace.TraceInformation("GetShallowathlete: " + name);

            var athletes = new List<ShallowTriathlete>();
            IDatabase cache;
            IEnumerable<SortedSetEntry> sortedAthletes; 
            try
            {
                cache = Connection.GetDatabase();
            
                sortedAthletes = cache.SortedSetScan(_CacheKeyShallowAthletes, String.Format("{0}*",name.ToLower()));
               
                foreach (var a in sortedAthletes.Take(50))
                {
                    var value = a.Element.ToString();
                    string[] fields = value.Split('#');
                    athletes.Add(
                                    new ShallowTriathlete
                                    {
                                        Name = fields[0],
                                        Id = Convert.ToInt32(fields[1]),
                                        RaceId = fields[2]
                        
                                    }
                        );
                }

            }

            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);

            }

            Trace.TraceInformation("leaving with # athletes: " + athletes.Count.ToString());
            return athletes;
        
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

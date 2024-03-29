﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RaceAnalysis.Models;
using HtmlAgilityPack;
using RestSharp;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace RaceAnalysis.Rest
{
    public class IronmanClient : RestClientX
    {
        private RaceAnalysisDbContext _DBContext;

        public IronmanClient(RaceAnalysisDbContext db) :base()
        {
            _DBContext = db;
        }
        public List<KeyValuePair<string, string>> BuildRequestParameters(int page, RequestContext req)
        {
            string pageNum = page.ToString();

            //this context might not be in the database, so we are using values to look up the info in other models
            Race race = _DBContext.Races.Single(r => r.RaceId == req.RaceId);
            string agegroupVal = _DBContext.AgeGroups.Single(a => a.AgeGroupId == req.AgeGroupId).Value;
            string genderVal = _DBContext.Genders.Single(g => g.GenderId == req.GenderId).Value;



            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("race", race.ShortName));
            parameters.Add(new KeyValuePair<string, string>("rd", race.RaceDate.ToString("yyyyMMdd")));
            parameters.Add(new KeyValuePair<string, string>("p", pageNum));
            parameters.Add(new KeyValuePair<string, string>("agegroup", agegroupVal));
            parameters.Add(new KeyValuePair<string, string>("sex", genderVal));


            return parameters;
        }

        public virtual List<Triathlete> ParseData(RequestContext request, string htmlData,int pageNum)
        {
          //  Trace.TraceInformation(htmlData);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlData);

            if(pageNum == 1)
                ParseDataToVerify(request,doc);

            var count = doc.DocumentNode
                 .Descendants("table")
                 .Where(t => t.Attributes["id"].Value == "eventResults").Count();


            var myTable = doc.DocumentNode
                 .Descendants("table")
                 .Where(t => t.Attributes["id"].Value == "eventResults")
                 .LastOrDefault();//changed to Last rather First because of bug in IMBoulder2016 where they show 2 grids, the 2nd one is the correct one

            if (myTable == null) //this case is expected when we've iterated through all of the pages of the source
            {
                if (request.SourceCount == 0) //we have retrieved nothing from this source
                {
                    request.Status = "Failed to find table in HTML document.";
                    request.Instruction = RequestInstruction.ForceSource; //force next request rather than go to cache
                    Trace.TraceError(request.Status);
                }

                return new List<Triathlete>();
            }



            var headers = myTable.Descendants("th");

            var body = myTable.Element("tbody");

            var trs = body.Descendants("tr");

            var data = from tr in trs
                       let tds = tr.Descendants("TD").ToArray()

                       select new Triathlete
                       {
                           RequestContext = request,
                           RequestContextId = request.RequestContextId,
                           Name = IronmanClient.ParseName(tds[0]),
                           Link = IronmanClient.ParseLink(tds[0]),
                           Country = tds[1].InnerText,
                           DivRank = ParseInt(tds[2].InnerText),
                           GenderRank = ParseInt(tds[3].InnerText),
                           OverallRank = ParseInt(tds[4].InnerText),
                           Swim = ParseTimeSpan(tds[5].InnerText),
                           Bike = ParseTimeSpan(tds[6].InnerText),
                           Run = ParseTimeSpan(tds[7].InnerText),
                           Finish = ParseTimeSpan(tds[8].InnerText),
                           Points = ParseInt(tds[9].InnerText)

                       };

            return data.ToList<Triathlete>();
        }


        public virtual void ParseDataToVerify(RequestContext request, string htmlData)
        {
            

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlData);
            ParseDataToVerify(request, doc);
        }

        public bool HandleResponse(IRestResponse response, RequestContext reqContext)
        {
            
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                reqContext.Status = "OK";

            }
            else if (response.ResponseStatus == ResponseStatus.Aborted)
            {

                reqContext.Status = "Aborted";

            }
            else if (response.ResponseStatus == ResponseStatus.Error)
            {

                reqContext.Status = "Error";

            }
            else if (response.ResponseStatus == ResponseStatus.TimedOut)
            {

                reqContext.Status = "Timed Out";

            }
            else if (response.ResponseStatus == ResponseStatus.None)
            {
                reqContext.Status = "None";


            }

        //  Move this info into the reqContext
            if (response.ErrorException != null) //TO-DO may not throw an exception here and just log it
            {
              //  const string message = "Error retrieving response.  Check inner details for more info.";
             //   var RaceAnalysisException = new ApplicationException(message, response.ErrorException);
              //  throw RaceAnalysisException;

            }

            return (response.ResponseStatus == ResponseStatus.Completed);
        }
        protected static int ParseInt(string s)
        {
            int result;
            if (int.TryParse(s, out result))
                return result;
            return 0;
        }

        protected static TimeSpan ParseTimeSpan(string s)
        {
            TimeSpan result;
            if (TimeSpan.TryParse(s, out result))
                return result;
            return result;
        }

        protected static String ParseLink(HtmlNode td)
        {

            var link = td.Descendants("a").First(x => x.Attributes["class"] != null
                                               && x.Attributes["class"].Value == "athlete");
            string hrefValue = link.Attributes["href"].Value;

            //ID = ....href.Split
            return hrefValue;
        }
        protected static String ParseName(HtmlNode td)
        {

            var link = td.Descendants("a").First(x => x.Attributes["class"] != null
                                               && x.Attributes["class"].Value == "athlete");
            return link.InnerHtml;


        }

        private void ParseDataToVerify(RequestContext request, HtmlDocument doc)
        {
            var tableDiv = doc.DocumentNode
                  .Descendants("div")
                  .Where(t => t.Attributes.Contains("class") && t.Attributes["class"].Value.Equals("results-athletes-table"))
                  .FirstOrDefault();
            if (tableDiv != null)
            {
                var h2Ele = tableDiv.Element("h2");
                var spanElement = h2Ele.Element("span");
                var val = spanElement.InnerHtml;
                var numericPart = Regex.Match(val, "\\d+").Value;
                int count = -1;
                int.TryParse(numericPart, out count);
                request.Expected = count;
            }
            else
            {
                Trace.TraceError("Didn't find tableDiv to verify count");
                request.Expected = -99; //magic number to indicate we didnt find tableDiv
            }
        }
    }//class


   public class IronmanClientDoubleTable : IronmanClient
   {
        public IronmanClientDoubleTable(RaceAnalysisDbContext db) : base(db) { }


        public override List<Triathlete> ParseData(RequestContext request, string htmlData,int pageNum)
        {

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlData);


            var count = doc.DocumentNode
                 .Descendants("table")
                 .Where(t => t.Attributes["id"].Value == "eventResults").Count();
            //we expect 2 tables. when there is only 1 left, that means that we have reached the end of this request context

            var myTable = doc.DocumentNode
                 .Descendants("table")
                 .Where(t => t.Attributes["id"].Value == "eventResults")
                 .LastOrDefault();//changed to Last rather First because of bug in IMBoulder2016 where they show 2 grids, the 2nd one is the correct one

            if (myTable == null || count < 2) //this case is expected when we've iterated through all of the pages of the source
            {
                if (request.SourceCount == 0) //we have retrieved nothing from this source
                {
                    request.Status = "Failed to find table in HTML document.";
                    request.Instruction = RequestInstruction.ForceSource; //force next request rather than go to cache
                }

                return new List<Triathlete>();
            }



            var headers = myTable.Descendants("th");

            var body = myTable.Element("tbody");

            var trs = body.Descendants("tr");

            var data = from tr in trs
                       let tds = tr.Descendants("TD").ToArray()

                       select new Triathlete
                       {
                           RequestContext = request,
                           RequestContextId = request.RequestContextId,
                           Name = IronmanClient.ParseName(tds[0]),
                           Link = IronmanClient.ParseLink(tds[0]),
                           Country = tds[1].InnerText,
                           DivRank = ParseInt(tds[2].InnerText),
                           GenderRank = ParseInt(tds[3].InnerText),
                           OverallRank = ParseInt(tds[4].InnerText),
                           Swim = ParseTimeSpan(tds[5].InnerText),
                           Bike = ParseTimeSpan(tds[6].InnerText),
                           Run = ParseTimeSpan(tds[7].InnerText),
                           Finish = ParseTimeSpan(tds[8].InnerText),
                           Points = ParseInt(tds[9].InnerText)

                       };

            return data.ToList<Triathlete>();
        }

    }
}
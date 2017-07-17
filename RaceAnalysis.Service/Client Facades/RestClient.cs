using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using RestSharp;
using RaceAnalysis.Models;

namespace RaceAnalysis.Rest
{
    public class RestClientX
    {
        public RestClientX()
        {
        }
     
        public IRestResponse MakeRequest(string requestUri, List<KeyValuePair<string, string>> parameters)
        {

            var client = new RestClient(requestUri);
            var request = new RestRequest();
            foreach (var parm in parameters)
            {
                request.AddParameter(parm.Key, parm.Value);
            }

         
            var response = client.Execute(request);

            
            return response;

        }
   
        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(requestUri);
                using (var content = new FormUrlEncodedContent(values))
                {
                    var query = await content.ReadAsStringAsync();
                    var requestUriWithQuery = string.Empty;                         //we can do this slicker
                    if (requestUri.Length > 0)
                        requestUriWithQuery = string.Concat(requestUri, "?", query);
                    else
                        requestUriWithQuery = requestUri;
                    var response = await client.GetAsync(requestUriWithQuery);
                    return response;
                }
            }

        }
        public async Task<String> GetContent(HttpResponseMessage httpResponse)
        {
            String content = String.Empty;
            if (httpResponse.Content != null)
            {
                return await httpResponse.Content.ReadAsStringAsync();
            }

            return content;
        }


    }
}

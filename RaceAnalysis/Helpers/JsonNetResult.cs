using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RaceAnalysis.Helpers
{
    public class JsonNetResult : JsonResult
    {
        new public object Data { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
               
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting.Indented };
                JsonSerializer serializer = new JsonSerializer();
                /*** this code was causing issues with the race name not getting populated for every row.
                JsonSerializer serializer = 
                    JsonSerializer.Create(  
                     
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
                    ***/
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}
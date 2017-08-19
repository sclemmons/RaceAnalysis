using Newtonsoft.Json;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

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
                Trace.TraceInformation("JsonNet-1");
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting.Indented };
               // JsonSerializer serializer = new JsonSerializer();
                /*** this code was causing issues with the race name not getting populated for every row.
                JsonSerializer serializer = 
                    JsonSerializer.Create(  
                     
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });
                    ***/

                JsonSerializer serializer =
                    JsonSerializer.Create(

                    new JsonSerializerSettings
                    {
                         ReferenceLoopHandling  = ReferenceLoopHandling.Ignore
                         
                    });

                serializer.Serialize(writer, Data);
                writer.Flush();
                Trace.TraceInformation("JsonNet-end");

            }
        }
    }
}
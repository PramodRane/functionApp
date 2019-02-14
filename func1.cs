using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    public static class func1
    {
        [FunctionName("func1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;


            var events = new List<EventGridEvent>();
            EventGridEvent eventGridEvent = new EventGridEvent();
            eventGridEvent.Topic = @"Names";
            eventGridEvent.Id = Guid.NewGuid().ToString();
            eventGridEvent.Subject = $"Name/{name?? "NotFound"}";
            eventGridEvent.EventType = !string.IsNullOrEmpty(name)? "NameFound": "NameNotFound";
            eventGridEvent.EventTime = DateTime.UtcNow;
            eventGridEvent.Data = name?? "NotFound";
            events.Add(eventGridEvent);
            var content = JsonConvert.SerializeObject(events);

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aeg-sas-key", "");
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            await client.PostAsync("http://localhost:5000/Names", httpContent);

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name} and data: {httpContent}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}

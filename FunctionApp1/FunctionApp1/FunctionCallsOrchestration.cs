using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace FunctionApp1
{
    public static class FunctionCallsOrchestration
    {
        [FunctionName("FunctionCallsOrchestration")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient orchClient,
            ILogger log)
        {


            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var orchesrtrationId = await orchClient.StartNewAsync(Orchestrators.RespondToNameConst, Guid.NewGuid().ToString(), name);

            return orchClient.CreateCheckStatusResponse(req, orchesrtrationId);
        }
    }
}

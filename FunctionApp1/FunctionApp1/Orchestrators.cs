using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FunctionApp1
{
    public static class Orchestrators
    {
        public const string RespondToNameConst = "O_RespondToName";
        [FunctionName(RespondToNameConst)]
        public static async Task<object> RespondToName(
            [OrchestrationTrigger] IDurableOrchestrationContext orchContext,
            ILogger log)
        {
            var input = orchContext.GetInput<string>();
            var firstActionOutput = await orchContext.CallActivityAsync<string>(Activities.GetFullNameConst, input);
            var secondActionOutput = await orchContext.CallActivityAsync<string>(Activities.GreetConst, firstActionOutput);

            log.LogInformation(secondActionOutput);

            return new
            {
                a = firstActionOutput,
                b = secondActionOutput
            };
        }
    }
}

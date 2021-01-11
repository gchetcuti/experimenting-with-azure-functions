using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public static class Activities
    {
        //kept private. Would put in config file if I was continuing.
        private const string GetFullNameUri = "";
        //kept private. Would put in config file if I was continuing.
        private const string GetGreetingUri = "";

        public const string GetFullNameConst = "A_GetFullName";
        [FunctionName(GetFullNameConst)]
        public static async Task<string> GetFullName(
            [ActivityTrigger] string input,
            ILogger logger)
        {
            if (input == "Grey" || input == "Gray")
            {
                return "Graeme Chetcuti";
            }

            if (input == "John" || input == "Jonny")
            {
                return "Agent John Smith";
            }

            if (input == "neo")
            {
                return "Mr Anderson";
            }

            //Won't work don't want to reveal my current db access details
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync($"{GetFullNameUri}?name={input}");
            var data = JsonConvert.DeserializeObject<FullName>(await result.Content.ReadAsStringAsync());
            return data.fullName;
        }

        public const string GreetConst = "A_Greet";
        [FunctionName(GreetConst)]
        public static async Task<string> Greet(
        [ActivityTrigger] string input,
        ILogger logger)
        {
            if (input == "Graeme Chetcuti")
                return "Hello Dolores Day";

            if (input.Contains("Anderson"))
            {
                return "Take the blue pill Neo!";
            }

            if (input.Contains("Agent"))
            {
                return "Goodbye Smith";
            }

            //Won't work. Don't want to reveal any of my details.
            var httpClient = new HttpClient();
            var result = await httpClient.GetAsync($"{GetGreetingUri}?name={input}");
            var data = JsonConvert.DeserializeObject<Greeting>(await result.Content.ReadAsStringAsync());
            return data.greeting;
        }
    }

    class Greeting
    {
        public string greeting;
    }

    class FullName
    {
        public string fullName;
    }
}
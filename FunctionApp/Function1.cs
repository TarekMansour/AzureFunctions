using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace FunctionApp
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {           
            var client = new HttpClient();
            var animalType = "cat";
            var animalAmount = 5;

            //call a public api to get animal daily facts
            string url = $"https://cat-fact.herokuapp.com/facts/random?animal_type={animalType}&amount={animalAmount}";            
            log.LogInformation("A request call to Daily cat facts is started.");

            var response = await client.GetAsync(url);
            var jsonString = response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<object>(jsonString.Result);

            log.LogInformation($"Api Response is: {model}");

            if (response.IsSuccessStatusCode)
            {
                return new OkObjectResult("call to Daily Cat facts endpoint successfully ended.");
            }
            else
            {
                return new BadRequestObjectResult("invalid response");
            }
        }
    }
}

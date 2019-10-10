using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp_fongdemo
{
    public static class FunctionTimerSample
    {
        [FunctionName("FunctionTimerSample")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var url = "";
            var results = await SendRequest(url);
            log.LogInformation(String.Format("AssetCreateModels: {0}", results));

        }

        public static async Task<string> SendRequest(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Environment.GetEnvironmentVariable("Auth_User")}:{Environment.GetEnvironmentVariable("Auth_Pass")}")));
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                //dictionary.Add("PARAM1", "VALUE1");
                //dictionary.Add("PARAM2", text);

                string json = JsonConvert.SerializeObject(dictionary);
                var requestData = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, requestData);
                var result = await response.Content.ReadAsStringAsync();

                return result;
            }
        }
    }
}

using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Function
{
    public class STFWindowsRequest
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("STFWindowsRequest")]
        public void Run([TimerTrigger("*/5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string location = Environment.GetEnvironmentVariable("LOCATION");
            string windowsAppUrl = String.Format(Helper.WindowsAppUrl, location);

            // Get the state of the site (empty, code deployed, or nonexistant)
            HttpResponseMessage response = Helper.SendRequest(client, windowsAppUrl, HttpMethod.Get);
           
            // Site will return custom 222 code when API is deployed, stop further requests otherwise
            if ((int)response.StatusCode == 222)
            {
                log.LogInformation("API deployed");
            }
            else
            {
                if (response.IsSuccessStatusCode)
                {
                    log.LogInformation("Empty site.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    log.LogInformation("Site not found");
                }
                else if ((int)response.StatusCode >= 500)
                {
                    log.LogError("Server Error");
                }

                return;
            }

            response = Helper.SendRequest(client, windowsAppUrl + "/PingVm", HttpMethod.Post);
            log.LogInformation("PingVm: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/StorageUpload", HttpMethod.Post);
            log.LogInformation("StorageUpload: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/PrivateSite", HttpMethod.Post);
            log.LogInformation("PrivateSite: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/PrivateScmSite", HttpMethod.Post);
            log.LogInformation("PrivateScmSite: " + response.StatusCode.ToString());
        }

    }

    public class STFLinuxRequest
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("STFLinuxRequest")]
        public void Run([TimerTrigger("*/5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string location = Environment.GetEnvironmentVariable("LOCATION");
            string linuxAppUrl = String.Format(Helper.LinuxAppUrl, location);

            // Get the state of the site (empty, code deployed, or nonexistant)
            HttpResponseMessage response = Helper.SendRequest(client, linuxAppUrl, HttpMethod.Get);

            // Site will return custom 222 code when API is deployed, stop further requests otherwise
            if ((int)response.StatusCode == 222)
            {
                log.LogInformation("API deployed");
            }
            else
            {
                if (response.IsSuccessStatusCode)
                {
                    log.LogInformation("Empty site.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    log.LogInformation("Site not found");
                }
                else if ((int)response.StatusCode >= 500)
                {
                    log.LogError("Server Error");
                }

                return;
            }

            response = Helper.SendRequest(client, linuxAppUrl + "/PingVm", HttpMethod.Post);
            log.LogInformation("PingVm: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/StorageUpload", HttpMethod.Post);
            log.LogInformation("StorageUpload: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/PrivateSite", HttpMethod.Post);
            log.LogInformation("PrivateSite: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/PrivateScmSite", HttpMethod.Post);
            log.LogInformation("PrivateScmSite: " + response.StatusCode.ToString());

        }

    }
}

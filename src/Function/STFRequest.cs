using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Hosting.Tracing;
using Newtonsoft.Json;

namespace Function
{
    public class STFWindowsRequest
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("STFWindowsRequest")]
        public static void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string location = Environment.GetEnvironmentVariable("LOCATION");
            string windowsAppUrl = String.Format(Helper.WindowsAppUrl, location);
            Uri windowsAppUri = new Uri(windowsAppUrl);

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
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(String.Format("Swift Test Framework site name resolution failed: {0}", windowsAppUrl), string.Empty);
                }
                else if ((int)response.StatusCode >= 500)
                {
                    log.LogError("Server Error");
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(String.Format("Swift Test Framework site internal server error: {0}", windowsAppUrl), string.Empty);
                }

                return;
            }

            TestSuiteResponse testSuiteResponse = new TestSuiteResponse();
            testSuiteResponse.Timestamp = DateTime.Now.ToString();
            testSuiteResponse.SiteName = windowsAppUri.Host.Split('.')[0];
            testSuiteResponse.Endpoints = new Dictionary<string, HttpStatusCode>();

            response = Helper.SendRequest(client, windowsAppUrl + "/PingVm", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PingVm", response.StatusCode);
            log.LogInformation("PingVm: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/StorageUpload", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("StorageUpload", response.StatusCode);
            log.LogInformation("StorageUpload: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/PrivateSite", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PrivateSite", response.StatusCode);
            log.LogInformation("PrivateSite: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/PrivateScmSite", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PrivateScmSite", response.StatusCode);
            log.LogInformation("PrivateScmSite: " + response.StatusCode.ToString());

            string jsonString = JsonConvert.SerializeObject(testSuiteResponse, Formatting.Indented);
            log.LogInformation(jsonString);

            AntaresEventProvider.EventWriteSwiftGenericLog(jsonString);
        }

    }

    public class STFLinuxRequest
    {
        static readonly HttpClient client = new HttpClient();

        [FunctionName("STFLinuxRequest")]
        public static void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string location = Environment.GetEnvironmentVariable("LOCATION");
            string linuxAppUrl = String.Format(Helper.LinuxAppUrl, location);
            Uri linuxAppUri = new Uri(linuxAppUrl);

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
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(String.Format("Swift Test Framework site name resolution failed: {0}", linuxAppUrl), string.Empty);

                }
                else if ((int)response.StatusCode >= 500)
                {
                    log.LogError("Server Error");
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(String.Format("Swift Test Framework site internal server error: {0}", linuxAppUrl), string.Empty);
                }

                return;
            }

            TestSuiteResponse testSuiteResponse = new TestSuiteResponse();
            testSuiteResponse.Timestamp = DateTime.Now.ToString();
            testSuiteResponse.SiteName = linuxAppUri.Host.Split('.')[0];
            testSuiteResponse.Endpoints = new Dictionary<string, HttpStatusCode>();

            response = Helper.SendRequest(client, linuxAppUrl + "/PingVm", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PingVm", response.StatusCode);
            log.LogInformation("PingVm: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/StorageUpload", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("StorageUpload", response.StatusCode);
            log.LogInformation("StorageUpload: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/PrivateSite", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PrivateSite", response.StatusCode);
            log.LogInformation("PrivateSite: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/PrivateScmSite", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PrivateScmSite", response.StatusCode);
            log.LogInformation("PrivateScmSite: " + response.StatusCode.ToString());

            string jsonString = JsonConvert.SerializeObject(testSuiteResponse, Formatting.Indented);
            log.LogInformation(jsonString);

            AntaresEventProvider.EventWriteSwiftGenericLog(jsonString);
        }

    }
}

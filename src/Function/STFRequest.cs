using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Authentication.ExtendedProtection;
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

            string projectName = Environment.GetEnvironmentVariable("ResourceGroup");
            string windowsAppUrl = String.Format(Helper.WindowsAppUrl, projectName);
            Uri windowsAppUri = new Uri(windowsAppUrl);
            string timestamp = DateTime.Now.ToString();
            string siteName = windowsAppUri.Host.Split('.')[0];

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
                    log.LogError("Site not found");
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    int statusCode = ((int)response.StatusCode);
                    ErrorResponse errorResponse = new ErrorResponse(Helper.ServiceName, timestamp, siteName, statusCode, responseContent);
                    string responseBody = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(responseBody, string.Empty);
                    log.LogInformation(responseBody);
                }
                else if ((int)response.StatusCode >= 500)
                {
                    log.LogError("Server Error");
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    int statusCode = ((int)response.StatusCode);
                    ErrorResponse errorResponse = new ErrorResponse(Helper.ServiceName, timestamp, siteName, statusCode, responseContent);
                    string responseBody = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(responseBody, string.Empty);
                    log.LogInformation(responseBody);
                }

                return;
            }

            TestSuiteResponse testSuiteResponse = new TestSuiteResponse(Helper.ServiceName, timestamp, siteName);

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

            response = Helper.SendRequest(client, windowsAppUrl + "/KeyVaultSecret", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("KeyVaultSecret", response.StatusCode);
            log.LogInformation("KeyVaultSecret: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, windowsAppUrl + "/PingPeeredVm", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PingPeeredVm", response.StatusCode);
            log.LogInformation("PingPeeredVm: " + response.StatusCode.ToString());

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

            string projectName = Environment.GetEnvironmentVariable("ResourceGroup");
            string linuxAppUrl = String.Format(Helper.LinuxAppUrl, projectName);
            Uri linuxAppUri = new Uri(linuxAppUrl);
            string timestamp = DateTime.Now.ToString();
            string siteName = linuxAppUri.Host.Split('.')[0];

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
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    int statusCode = ((int)response.StatusCode);
                    ErrorResponse errorResponse = new ErrorResponse(Helper.ServiceName, timestamp, siteName, statusCode, responseContent);
                    string responseBody = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(responseBody, string.Empty);
                    log.LogInformation(responseBody);

                }
                else if ((int)response.StatusCode >= 500)
                {
                    log.LogError("Server Error");
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    int statusCode = ((int)response.StatusCode);
                    ErrorResponse errorResponse = new ErrorResponse(Helper.ServiceName, timestamp, siteName, statusCode, responseContent);
                    string responseBody = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);
                    AntaresEventProvider.EventWriteSwiftWarningWithVnetId(responseBody, string.Empty);
                    log.LogInformation(responseBody);
                }

                return;
            }

            TestSuiteResponse testSuiteResponse = new TestSuiteResponse(Helper.ServiceName, timestamp, siteName);

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

            response = Helper.SendRequest(client, linuxAppUrl + "/KeyVaultSecret", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("KeyVaultSecret", response.StatusCode);
            log.LogInformation("KeyVaultSecret: " + response.StatusCode.ToString());

            response = Helper.SendRequest(client, linuxAppUrl + "/PingPeeredVm", HttpMethod.Post);
            testSuiteResponse.Endpoints.Add("PingPeeredVm", response.StatusCode);
            log.LogInformation("PingPeeredVm: " + response.StatusCode.ToString());

            string jsonString = JsonConvert.SerializeObject(testSuiteResponse, Formatting.Indented);
            log.LogInformation(jsonString);

            AntaresEventProvider.EventWriteSwiftGenericLog(jsonString);
        }

    }
}

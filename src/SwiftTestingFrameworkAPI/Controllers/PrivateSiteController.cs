using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;
using System;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("PrivateSite")]
    public class PrivateSiteController : ControllerBase
    {

        private readonly ILogger<PrivateSiteController> _logger;

        private const string TestName = "PrivateSite";

        static readonly HttpClient client = new HttpClient();

        public PrivateSiteController(ILogger<PrivateSiteController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Send HTTP requests to site of app that is connected with a Private Endpoint.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult RequestSite()
        {
            string projectName = Environment.GetEnvironmentVariable("ResourceGroup") ?? String.Empty;
            string siteHostname = String.Format(Constants.PrivateSiteHostname, projectName);
            Helper.ProcessOutput p;
            TestResponse testResponse;

            try
            {
                HttpResponseMessage response = Helper.SendRequest(client, "http://" + siteHostname, HttpMethod.Get);
                if (!response.IsSuccessStatusCode)
                {
                    testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", "Http request failed", response.Content.ReadAsStringAsync().Result);
                    return StatusCode(555, testResponse);
                }

                if (OperatingSystem.IsWindows())
                {
                    p = Helper.StartProcess("nameresolver.exe", siteHostname + " " + Constants.AzureDNS);
                }
                else
                {
                    p = Helper.StartProcess("nslookup", siteHostname + " " + Constants.AzureDNS);
                }

                if (p.ExitCode == 0)
                {
                    if (p.StdOutput.Contains("privatelink"))
                    {
                        testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", p.StdOutput, string.Empty);
                        return StatusCode(200, testResponse);
                    }
                    else
                    {
                        testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, "Privatelink CNAME for site does not exist.");
                        return StatusCode(555, testResponse);
                    }
                }
                else
                {
                    testResponse =  new TestResponse(Constants.ApiVersion, TestName, "Failure", "Exit Code not 0", p.StdError);
                    return StatusCode(555, testResponse);
                }
            }
            catch (Exception ex)
            {
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, ex.Message + ex.StackTrace);
                return StatusCode(555, testResponse);
            }

        }
    }

    [ApiController]
    [Route("PrivateScmSite")]
    public class PrivateScmSiteController : ControllerBase
    {

        private readonly ILogger<PrivateScmSiteController> _logger;

        private const string TestName = "PrivateScmSite";

        public PrivateScmSiteController(ILogger<PrivateScmSiteController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Send HTTP requests to the SCM site of an app that is connected with a Private Endpoint.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult RequestScmSite()
        {
            string projectName = Environment.GetEnvironmentVariable("ResourceGroup") ?? String.Empty;
            string scmHostname = String.Format(Constants.PrivateSiteScmHostname, projectName);
            Helper.ProcessOutput p;
            TestResponse testResponse;

            try
            {
                if (OperatingSystem.IsWindows())
                {
                    p = Helper.StartProcess("nameresolver.exe", scmHostname + " " + Constants.AzureDNS);
                }
                else
                {
                    p = Helper.StartProcess("nslookup", scmHostname + " " + Constants.AzureDNS);
                }

                if (p.ExitCode == 0)
                {
                    if (p.StdOutput.Contains("privatelink"))
                    {
                        testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", p.StdOutput, string.Empty);
                        return StatusCode(200, testResponse);
                    }
                    else
                    {
                        testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, "Privatelink CNAME does not exist for site");
                        return StatusCode(555, testResponse);
                    }
                }
                else
                {
                    testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", "Exit code not 0", p.StdError);
                    return StatusCode(555, testResponse);
                }
            }
            catch (Exception ex)
            {
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, ex.Message + ex.StackTrace);
                return StatusCode(555, testResponse);
            }

        }
    }
}
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
            string location = Environment.GetEnvironmentVariable("LOCATION") ?? String.Empty;
            string siteHostname = String.Format(Constants.PrivateSiteHostname, location);
            string scmHostname = String.Format(Constants.PrivateSiteScmHostname, location);
            Helper.ProcessOutput p;
            TestResponse testResponse;

            try
            {
                p = Helper.StartProcess("nameresolver.exe", siteHostname + " " + Constants.AzureDNS);

                if (p.ExitCode == 0)
                {
                    if (p.StdOutput.Contains("privatelink"))
                    {
                        testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", p.StdOutput, string.Empty);
                        return StatusCode(200, testResponse);
                    }
                    else
                    {
                        testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, "Did not resolve through privatelink");
                        return StatusCode(555, testResponse);
                    }
                }
                else
                {
                    testResponse =  new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, siteHostname);
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
        public TestResponse RequestScmSite()
        {
            string location = Environment.GetEnvironmentVariable("LOCATION") ?? String.Empty;
            string scmHostname = String.Format(Constants.PrivateSiteScmHostname, location);
            Helper.ProcessOutput p;

            try
            {
                p = Helper.StartProcess("nameresolver.exe", scmHostname + " " + Constants.AzureDNS);

                if (p.ExitCode == 0)
                {
                    if (p.StdOutput.Contains("privatelink"))
                    {
                        return new TestResponse(Constants.ApiVersion, TestName, "Success", p.StdOutput, string.Empty);
                    }
                    else
                    {
                        return new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, "Did not resolve through privatelink");
                    }
                }
                else
                {
                    return new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, p.StdError);
                }
            }
            catch (Exception ex)
            {
                return new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, ex.Message + ex.StackTrace);
            }

        }
    }
}
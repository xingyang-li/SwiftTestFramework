using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;
using System;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("PrivatePort")]
    public class PrivatePortsController : ControllerBase
    {

        private readonly ILogger<PrivatePortsController> _logger;

        private const string TestName = "PrivatePort";

        static readonly HttpClient client = new HttpClient();

        public PrivatePortsController(ILogger<PrivatePortsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string stringAddr = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_IP") ?? string.Empty;
            string stringPort = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_PORTS") ?? string.Empty;
            string testDetails = String.Format("Ping the private port of another instance hosting this application. Private IP: {0}, Private Port: {1}", stringAddr, stringPort);
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult RequestSite()
        {
            Helper.ProcessOutput p;
            TestResponse testResponse;

            string stringAddr = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_IP") ?? string.Empty;
            string stringPort = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_PORTS") ?? string.Empty;

            try
            {
                p = Helper.StartProcess("tcpping.exe", stringAddr + ":" + stringPort);

                if (p.ExitCode == 0)
                {
                    testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", p.StdOutput, string.Empty);
                    return StatusCode(200, testResponse);
                }
                else
                {
                    testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, p.StdError);
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
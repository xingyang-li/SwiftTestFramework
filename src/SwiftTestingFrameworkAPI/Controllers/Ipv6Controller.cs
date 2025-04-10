using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("Ipv6")]
    public class Ipv6Controller : ControllerBase
    {

        private readonly ILogger<Ipv6Controller> _logger;

        private const string TestName = "Ipv6";

        public Ipv6Controller(ILogger<Ipv6Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Establishes a TCP connection with a public IPv6 address. (ipv6.google.com)";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult SendIpv6Request()
        {
            Helper.ProcessOutput p;
            TestResponse testResponse;
            try
            {
 
                if (OperatingSystem.IsWindows())
                {
                    p = Helper.StartProcess("tcpping.exe", Constants.PublicIpv6Endpoint);
                }
                else
                {
                    p = Helper.StartProcess("curl", $"--connect-timeout 5 {Constants.PublicIpv6Endpoint}");
                }

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
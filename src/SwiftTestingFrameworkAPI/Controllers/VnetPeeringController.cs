using Azure.Core;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("PingPeeredVM")]
    public class VnetPeeringController : ControllerBase
    {

        private readonly ILogger<VnetPeeringController> _logger;

        private const string TestName = "PingPeeredVM";

        public VnetPeeringController(ILogger<VnetPeeringController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Send TCP ping to VM that is in a peered VNET";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult PeerVM()
        {
            Helper.ProcessOutput p;
            TestResponse testResponse;
            try
            {

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    p = Helper.StartProcess("tcpping.exe", Constants.PeeredVmAddress);
                }
                else
                {
                    p = Helper.StartProcess("curl", Constants.PeeredVmAddress);
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
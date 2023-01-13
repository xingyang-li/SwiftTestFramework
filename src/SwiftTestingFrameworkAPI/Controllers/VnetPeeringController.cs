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
        public TestResponse PeerVM()
        {
            Helper.ProcessOutput p;
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
                    return new TestResponse(Constants.ApiVersion, TestName, "Success", p.StdOutput, string.Empty);
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
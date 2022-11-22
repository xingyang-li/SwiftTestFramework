using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("PingVM")]
    public class PingVMController : ControllerBase
    {

        private readonly ILogger<PingVMController> _logger;

        private const string TestName = "PingVM";

        public PingVMController(ILogger<PingVMController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Establishes a TCP connection with VNET virtual machine's private IP address.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult PingVM()
        {
            Helper.ProcessOutput p;
            TestResponse testResponse;
            try
            {
 
                if (OperatingSystem.IsWindows())
                {
                    p = Helper.StartProcess("tcpping.exe", Constants.VmAddress);
                }
                else
                {
                    p = Helper.StartProcess("curl", Constants.VmAddress);
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
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
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, string.Empty);
        }

        [HttpPost]
        public TestResponse PingVM()
        {
            Helper.ProcessOutput p;
            try
            {
 
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    p = Helper.StartProcess("tcpping.exe", Constants.VmAddress);
                }
                else
                {
                    p = Helper.StartProcess("curl", Constants.VmAddress);
                }

                if (p.ExitCode == 0)
                {
                    return new TestResponse(Constants.ApiVersion, TestName, "Success", string.Empty);
                }
                else
                {
                    return new TestResponse(Constants.ApiVersion, TestName, "Failure", p.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                return new TestResponse(Constants.ApiVersion, TestName, "Failure", ex.Message + ex.StackTrace);
            }
        }
    }
}
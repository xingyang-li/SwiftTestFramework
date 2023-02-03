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
            string testDetails = "Ping the private port of another instance hosting this application.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult RequestSite()
        {
            TestResponse testResponse = new TestResponse(Constants.ApiVersion, TestName, string.Empty, string.Empty, string.Empty);
            return StatusCode(200, testResponse);
        }
    }
}
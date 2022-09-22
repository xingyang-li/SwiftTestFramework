using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;
using System.Net.Http;

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
            string testDetails = "Send HTTP requests to a site that is connected with a Private Endpoint.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public TestResponse RequestSite()
        {
            HttpRequestMessage message = new HttpRequestMessage();
            HttpResponseMessage response = Helper.SendRequest(client, Constants.WindowsAppUrl + testPath, HttpMethod.Post);
            response.EnsureSuccessStatusCode();
            string stringBody = response.Content.ReadAsStringAsync().Result;
            TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
            Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
        }
    }
}
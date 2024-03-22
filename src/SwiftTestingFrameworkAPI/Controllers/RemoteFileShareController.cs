using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;
using System;
using System.IO;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("FileShare")]
    public class RemoteFileShareController : ControllerBase
    {
        private readonly ILogger<RemoteFileShareController> _logger;
        private const string TestName = "FileShare";

        public RemoteFileShareController(ILogger<RemoteFileShareController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Write to file share mount through Vnet Integration";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult WriteToFileShare()
        {
            TestResponse testResponse;
            try
            {
                System.IO.File.WriteAllText(Constants.MountFilePath, "Hello world!");
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", "Write to remote file share successful.", string.Empty);
                return StatusCode(200, testResponse);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, ex.Message + ex.StackTrace);
                return StatusCode(555, testResponse);
            }
        }
    }
}

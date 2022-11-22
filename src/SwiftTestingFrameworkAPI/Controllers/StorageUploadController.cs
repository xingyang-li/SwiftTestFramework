using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("StorageUpload")]
    public class StorageUploadController : ControllerBase
    {
        private readonly ILogger<StorageUploadController> _logger;
        private const string TestName = "StorageUpload";

        public StorageUploadController(ILogger<StorageUploadController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Uploads an empty blob into a storage container through a private endpoint connection.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult StorageUpload()
        {
            TestResponse testResponse;
            try
            {
                PageBlobClient pageBlobClient = Helper.GetPageBlobClient("TestBlob");
                var response = pageBlobClient.Create(512);
                if (response.GetRawResponse().Status == 201)
                {
                    string details = "Blob uploaded sucessfully at " + response.Value.LastModified.ToString();
                    testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", details, string.Empty);
                    return StatusCode(200, testResponse);
                }
                else
                {
                    testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, response.GetRawResponse().Content.ToString());
                    return StatusCode(555, testResponse);   
                }
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

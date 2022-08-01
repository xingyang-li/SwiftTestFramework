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
        private readonly ILogger<PingVMController> _logger;
        private const string TestName = "StorageUpload";

        public StorageUploadController(ILogger<PingVMController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, string.Empty);
        }

        [HttpPost]
        public TestResponse StorageUpload()
        {
            try
            {
                PageBlobClient pageBlobClient = Helper.GetPageBlobClient("TestBlob");
                var response = pageBlobClient.Create(512);
                if (response.GetRawResponse().Status == 201)
                {
                    return new TestResponse(Constants.ApiVersion, TestName, "Success", string.Empty);
                }
                else
                {
                    return new TestResponse(Constants.ApiVersion, TestName, "Failure", response.GetRawResponse().Content.ToString());
                }
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                return new TestResponse(Constants.ApiVersion, TestName, "Failure", ex.Message + ex.StackTrace);
            }
        }
    }
}

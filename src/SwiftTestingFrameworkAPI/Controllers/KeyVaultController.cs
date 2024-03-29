﻿using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;
using Newtonsoft.Json;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("KeyVaultSecret")]
    public class KeyVaultController : ControllerBase
    {
        private readonly ILogger<KeyVaultController> _logger;
        private const string TestName = "KeyVaultSecret";

        public KeyVaultController(ILogger<KeyVaultController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Get this site's Key Vault secret references.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult KeyVaultSecret()
        {
            Dictionary<string, string> references = new Dictionary<string, string>();
            TestResponse testResponse;
            string secret1value = Environment.GetEnvironmentVariable("secret1") ?? string.Empty;

            if (secret1value.Contains("@Microsoft.KeyVault") || String.IsNullOrEmpty(secret1value))
            {
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, "Reference not resolved.");
                return StatusCode(555, testResponse);
            }
            else
            {
                references.Add("secret1", secret1value);
                string referencesJson = JsonConvert.SerializeObject(references);
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", referencesJson, string.Empty);
                return StatusCode(200, testResponse);
            }
        }
    }
}

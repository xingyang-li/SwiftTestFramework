﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SwiftTestingFrameworkAPI.Utils;


namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("SqlQuery")]
    public class SqlController : ControllerBase
    {
        private readonly ILogger<SqlController> _logger;
        private const string TestName = "SqlQuery";

        public SqlController(ILogger<SqlController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string testDetails = "Creates a table if it does not already exist in a SQL database and inserts a datetime entry. Done through a private endpoint connection.";
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult SqlConnect()
        {
            TestResponse testResponse;
            try
            {
                string queryResult = Helper.ExecuteSqlQuery();
                string details = "SQL table entry inserted successfully at " + queryResult;
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", details, string.Empty);
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

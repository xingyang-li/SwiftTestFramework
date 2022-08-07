using Microsoft.AspNetCore.Mvc;
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
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, string.Empty, string.Empty);
        }

        [HttpPost]
        public TestResponse SqlConnect()
        {
            try
            {
                string queryResult = Helper.ExecuteSqlQuery();
                return new TestResponse(Constants.ApiVersion, TestName, "Success", queryResult, string.Empty);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                return new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, ex.Message + ex.StackTrace);
            }
        }
    }
}

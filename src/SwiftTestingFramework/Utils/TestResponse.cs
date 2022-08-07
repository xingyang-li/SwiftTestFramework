namespace SwiftTestingFramework.Utils
{
    public class TestResponse
    {
        public string TestName { get; set; }

        public string ApiVersion { get; set; }

        public string TestResult { get; set; }

        public string Details { get; set; }

        public string ErrorMessage { get; set; }

        public TestResponse(string apiVersion, string testName, string testResult, string details, string errorMessage)
        {
            TestName = testName;
            ApiVersion = apiVersion;
            TestResult = testResult;
            Details = details;
            ErrorMessage = errorMessage;
        }
    }
}
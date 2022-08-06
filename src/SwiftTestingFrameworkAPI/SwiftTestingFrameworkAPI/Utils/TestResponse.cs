namespace SwiftTestingFrameworkAPI.Utils
{
    public class TestResponse
    {
        public string TestName { get; set; }

        public string ApiVersion { get; set; }

        public string TestResult { get; set; }

        public string ErrorMessage { get; set; }

        // add field for summary/ result of test action

        public TestResponse(string apiVersion, string testName, string testResult, string errorMessage)
        {
            TestName = testName;
            ApiVersion = apiVersion;
            TestResult = testResult;
            ErrorMessage = errorMessage;
        }
    }
}
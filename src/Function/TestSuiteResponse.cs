using System.Collections.Generic;
using System.Net;

namespace Function
{
    public class TestSuiteResponse
    {
        public string Service { get; set; }
        public string Timestamp { get; set; }
        public string SiteName { get; set; }
        public Dictionary<string, HttpStatusCode> Endpoints { get; set; }

        public TestSuiteResponse(string service, string timestamp, string siteName)
        {
            Service = service;
            Timestamp = timestamp;
            SiteName = siteName;
            Endpoints = new Dictionary<string, HttpStatusCode>();
        }
    }

    public class ErrorResponse
    {
        public string Service { get; set; }
        public string Timestamp { get; set; }
        public string SiteName { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string service, string timestamp, string siteName, string message)
        {
            Service = service;
            Timestamp = timestamp;
            SiteName = siteName;
            Message = message;
        }
    }
}


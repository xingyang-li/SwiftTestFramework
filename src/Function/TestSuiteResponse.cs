using System.Collections.Generic;
using System.Net;

namespace Function
{
    public class TestSuiteResponse
    {
        public string Timestamp { get; set; }
        public string SiteName { get; set; }
        public Dictionary<string, HttpStatusCode> Endpoints { get; set; }
    }
}


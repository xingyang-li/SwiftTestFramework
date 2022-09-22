#pragma warning disable CS8600, CS8602

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Newtonsoft.Json;
using SwiftTestingFramework.Utils;
using System.Net.Http;
using System.Reflection;

namespace SwiftTestingFramework 
{
    [TestClass]
    public class SwiftTests
    {
        static readonly HttpClient client = new HttpClient();

        [TestMethod]
        [Description("Run TcpPing against VM from a Windows App")]
        public void TestVMConnectionWindows()
        {
            try
            {
                string testPath = "/PingVM";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.WindowsAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
                Logger.LogMessage("Test Passed");
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }
        }

        [TestMethod]
        [Description("Send a curl request to VM from a Linux App.")]
        public void TestVMConnectionLinux()
        {
            try
            {
                string testPath = "/PingVM";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.LinuxAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }
        }

        [TestMethod]
        [Description("Upload an empty 512B page blob to a container in the storage account")]
        public void TestStorageUpload()
        {
            try
            {
                string testPath = "/StorageUpload";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.WindowsAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }

        }

        [TestMethod]
        [Description("Send requests to site of app connected with Private Endpoint")]
        public void TestSitePrivatelink()
        {
            try
            {
                string testPath = "/PrivateSite";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.WindowsAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }

        }

        [TestMethod]
        [Description("Send requests to the SCM site of app connected with Private Endpoint")]
        public void TestScmSitePrivatelink()
        {
            try
            {
                string testPath = "/PrivateScmSite";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.WindowsAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }

        }

        /*
        [TestMethod]
        [Description("Insert an entry inside a SQL Database table from Windows App")]
        public void TestDatabaseConnectionWindows()
        {
#if (westcentralus || northcentralus)
            Assert.Inconclusive();
#endif
            try
            {
                string testPath = "/SqlQuery";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.WindowsAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }

        }

        [TestMethod]
        [Description("Insert an entry inside a SQL Database table from Linux App")]
        public void TestDatabaseConnectionLinux()
        {
#if (westcentralus || northcentralus)
            Assert.Inconclusive();
#endif
            try
            {
                string testPath = "/SqlQuery";
                HttpRequestMessage message = new HttpRequestMessage();
                HttpResponseMessage response = Helper.SendRequest(client, Constants.LinuxAppUrl + testPath, HttpMethod.Post);
                response.EnsureSuccessStatusCode();
                string stringBody = response.Content.ReadAsStringAsync().Result;
                TestResponse testResponse = JsonConvert.DeserializeObject<TestResponse>(stringBody);
                Assert.AreEqual(testResponse.TestResult, "Success", testResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                //Replace with AntaresEventProvider or email send functionality
                Logger.LogMessage(ex.ToString());
                throw;
            }

        }
        */
    }
}


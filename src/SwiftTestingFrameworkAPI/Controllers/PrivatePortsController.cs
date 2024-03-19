using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SwiftTestingFrameworkAPI.Utils;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SwiftTestingFrameworkAPI.Controllers
{
    [ApiController]
    [Route("PrivatePort")]
    public class PrivatePortsController : ControllerBase
    {

        private readonly ILogger<PrivatePortsController> _logger;

        private const string TestName = "PrivatePort";

        static readonly HttpClient client = new HttpClient();

        public PrivatePortsController(ILogger<PrivatePortsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public TestResponse GetInfo()
        {
            string stringAddr = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_IP") ?? string.Empty;
            string stringPort = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_PORTS") ?? string.Empty;
            string testDetails = String.Format("Ping the private port of another instance hosting this application. Private IP: {0}, Private Port: {1}", stringAddr, stringPort);
            return new TestResponse(Constants.ApiVersion, TestName, string.Empty, testDetails, string.Empty);
        }

        [HttpPost]
        public ObjectResult RequestSite()
        {
            Helper.ProcessOutput p;
            TestResponse testResponse;

            string stringAddr = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_IP") ?? string.Empty;
            string stringPort = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_PORTS") ?? string.Empty;
            IPAddress hostIP = IPAddress.Parse(stringAddr);
            IPEndPoint ep = new IPEndPoint(hostIP.Address, Int32.Parse(stringPort));

            try
            {
                using Socket client = new (
                    ep.AddressFamily,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );

                client.Connect(ep);
                while (true)
                {
                    // Send message.
                    var message = "Hello world! <|EOM|>";
                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    client.Send(messageBytes, SocketFlags.None);
                    Console.WriteLine($"Socket client sent message: \"{message}\"");

                    // Receive ack.
                    var buffer = new byte[1_024];
                    var received = client.Receive(buffer, SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);
                    if (response == "<|ACK|>")
                    {
                        Console.WriteLine(
                            $"Socket client received acknowledgment: \"{response}\"");
                        break;
                    }
                }

                client.Shutdown(SocketShutdown.Both);

                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Success", "Message Sent", string.Empty);
                return StatusCode(200, testResponse);
            }
            catch (Exception ex)
            {
                testResponse = new TestResponse(Constants.ApiVersion, TestName, "Failure", string.Empty, ex.Message + ex.StackTrace);
                return StatusCode(555, testResponse);
            }
        }
    }
}
﻿using System.Net.Sockets;
using System.Net;
using System.Text;

namespace SwiftTestingFrameworkAPI
{
    public sealed class LifetimeService : IHostedService
    {
        private readonly ILogger _logger;

        private Socket listenSocket;

        public LifetimeService(
            ILogger<LifetimeService> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;

            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartAsync has been called.");
            Task.Run(async () => { await ListenOnPrivatePort(); });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync has been called.");

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called.");
        }

        private void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called.");
        }

        private void OnStopped()
        {
            listenSocket.Close();
            _logger.LogInformation("OnStopped has been called.");
        }

        public async Task ListenOnPrivatePort()
        {
            // Add services to the container.
            listenSocket = new Socket(AddressFamily.InterNetwork,
                                                    SocketType.Stream,
                                                    ProtocolType.Tcp);

            // bind the listening socket to the port
            string stringAddr = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_IP") ?? string.Empty;
            string stringPort = Environment.GetEnvironmentVariable("WEBSITE_PRIVATE_PORTS") ?? string.Empty;
            IPAddress hostIP = IPAddress.Parse(stringAddr);
            IPEndPoint ep = new IPEndPoint(hostIP.Address, Int32.Parse(stringPort));
            listenSocket.Bind(ep);

            // start listening
            listenSocket.Listen();

            while (true)
            {

                var handler = await listenSocket.AcceptAsync();
                // Receive message.
                var buffer = new byte[1_024];
                var received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                var response = Encoding.UTF8.GetString(buffer, 0, received);

                var eom = "<|EOM|>";
                if (response.IndexOf(eom) > -1 /* is end of message */)
                {
                    Console.WriteLine(
                        $"Socket server received message: \"{response.Replace(eom, "")}\"");

                    var ackMessage = "<|ACK|>";
                    var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await handler.SendAsync(echoBytes, 0);
                    Console.WriteLine(
                        $"Socket server sent acknowledgment: \"{ackMessage}\"");

                    handler.Close();
                }
            }
        }
    }
}

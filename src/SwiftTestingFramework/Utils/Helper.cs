using System;
using System.Diagnostics;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;

namespace SwiftTestingFramework.Utils
{
    public class Helper
    {
        public static HttpResponseMessage SendRequest(HttpClient client, string url, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            HttpResponseMessage response = null;

            for (int i = 0; i<Constants.MaxRetries; ++i)
            {
                response = client.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return response;
        }
    }
}

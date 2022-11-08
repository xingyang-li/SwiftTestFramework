using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Function
{
    public class Helper
    {
        public const string WindowsAppUrl = "https://stf-{0}-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://stf-{0}-linuxapp.azurewebsites.net";

        public static HttpResponseMessage SendRequest(HttpClient client, string url, HttpMethod method)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, url);
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response;
            }
            catch
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }

        }
    }
}

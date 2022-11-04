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

        public static HttpResponseMessage SendRequest(HttpClient client, string url, HttpMethod method, ILogger log)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, url);
                HttpResponseMessage response = client.Send(request);
                return response;
            }
            catch (HttpRequestException ex)
            {
                log.LogInformation(ex.Message);
                return new HttpResponseMessage(ex.StatusCode ?? System.Net.HttpStatusCode.InternalServerError);
            }

        }
    }
}

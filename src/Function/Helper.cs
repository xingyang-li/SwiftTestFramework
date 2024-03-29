﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Function
{
    public class Helper
    {
        public const string WindowsAppUrl = "https://{0}-winapp.azurewebsites.net";
        public const string LinuxAppUrl = "https://{0}-linuxapp.azurewebsites.net";
        public const string ServiceName = "Swift Test Framework";

        public static HttpResponseMessage SendRequest(HttpClient client, string url, HttpMethod method)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, url);
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response;
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    if (ex.GetType() == typeof(HttpRequestException))
                    {
                        HttpRequestException httpEx = (HttpRequestException)ex;
                        if (httpEx.InnerException.GetType() == typeof(WebException))
                        {
                            WebException webEx = (WebException)httpEx.InnerException;
                            if (webEx.Status == WebExceptionStatus.NameResolutionFailure || webEx.Status == WebExceptionStatus.ConnectFailure)
                            {
                                HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                                responseMessage.Content = new StringContent(webEx.Message);
                                return responseMessage;
                            }
                        }
                    }
                }

                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ae.Message);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(ex.Message);
                return response;
            }

        }
    }
}

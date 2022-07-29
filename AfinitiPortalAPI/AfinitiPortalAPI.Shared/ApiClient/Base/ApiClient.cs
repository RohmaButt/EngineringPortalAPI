using AfinitiPortalAPI.Shared.Shared.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.ApiClient
{
    public class ApiClient
    {
        protected readonly AppSettings AppSettings;
        protected string ApiBaseUrl { get; set; }

        public ApiClient(IOptions<AppSettings> configuration)
        {
            AppSettings = configuration.Value;
        }

        protected virtual async Task<(string ResponseBody, HttpStatusCode ResponseCode)> PerformCall(HttpMethod httpMethod, string pathAndQuery, string request = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);

                    // Headers...
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var uri = new Uri(new Uri(this.ApiBaseUrl), pathAndQuery);
                    HttpResponseMessage response = null;

                    // Perform api call...
                    if (httpMethod == HttpMethod.Get)
                    {
                        response = await client.GetAsync(uri);
                    }
                    else if (httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
                    {
                        var msg = new HttpRequestMessage(httpMethod, uri);
                        msg.Content = new StringContent(request, Encoding.UTF8, "application/json");
                        response = await client.SendAsync(msg, HttpCompletionOption.ResponseHeadersRead);
                    }
                    else if (httpMethod == HttpMethod.Delete)
                    {
                        response = await client.DeleteAsync(uri);
                    }
                    else
                    {
                        // Other HttpMethods are not implemented. We do not need them yet.
                    }

                    //Reading result & returning...
                    var result = string.Empty;
                    HttpContent content = response.Content;
                    using (Task<string> completedTask = content.ReadAsStringAsync())
                    {
                        result = completedTask.Result;
                        completedTask.Dispose();
                    }

                    return (result, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                Log.Error("{@jobName} | Unable to execute PerformCall(). Ex: {@ex}", this.GetType().Name, ex);
            }

            return (null, HttpStatusCode.InternalServerError);
        }
    }
}

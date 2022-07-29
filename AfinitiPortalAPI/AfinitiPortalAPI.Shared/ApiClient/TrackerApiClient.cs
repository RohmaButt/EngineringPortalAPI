using AfinitiPortalAPI.Shared.Extensions;
using AfinitiPortalAPI.Shared.Library.TrackerApi;
using AfinitiPortalAPI.Shared.Library.TrackerApi.Model;
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
    public class TrackerApiClient : ApiClient
    {
        public TrackerApiClient(IOptions<AppSettings> configuration) : base(configuration)
        {
            this.ApiBaseUrl = this.AppSettings.TrackerApi.BaseApiUrl;
        }

        public virtual async Task CreateAuditLog(string email, string path, string apiUrl, string domSelector, string clientIp, string ClientBrowser, string actionCode, string properties)
        {
            var requestBody = new AuditModel()
            {
                Email = email,
                ActionCode = actionCode,
                ActionType = ActionType.ConsumeApi,
                ApiUrl = apiUrl,
                ClientBrowser = ClientBrowser,
                ClientIp = clientIp,
                DomSelector = domSelector,
                Path = path,
                Properties = properties
            };

            await this.PerformCall(HttpMethod.Post, "audit?fromApi=true", requestBody.ToJson());
        }
    }
}

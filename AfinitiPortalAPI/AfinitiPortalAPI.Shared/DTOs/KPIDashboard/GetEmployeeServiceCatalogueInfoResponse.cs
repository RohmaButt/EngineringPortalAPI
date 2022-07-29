using AfinitiPortalAPI.Shared.Enums;
using AfinitiPortalAPI.Shared.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class GetEmployeeServiceCatalogueInfoResponse : GetResponse<List<GetEmployeeServiceCatalogueInfoResponseItem>>
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class GetEmployeeServiceCatalogueInfoResponseItem
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("portfolio")]
        public string Portfolio { get; set; }

        [JsonProperty("serviceGroup")]
        public string ServiceGroup { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("portfolioUrl")]
        public string PortfolioUrl { get; set; }

        [JsonProperty("serviceGroupUrl")]
        public string ServiceGroupUrl { get; set; }

        [JsonProperty("serviceUrl")]
        public string ServiceUrl { get; set; }
    }
}

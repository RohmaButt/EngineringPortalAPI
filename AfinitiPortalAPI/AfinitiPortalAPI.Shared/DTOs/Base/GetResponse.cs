using AfinitiPortalAPI.Shared.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.DTOs
{
    public class GetResponse<T>: ApiResponse
    {
        [JsonProperty("data")]
        public T Data { get; set; } = Activator.CreateInstance<T>();
    }
}

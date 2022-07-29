using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.Extensions
{
    public static class SerializationExtensions
    {
        public static string ToJson<T>(this T input, JsonSerializerSettings serializerSettings = null)
        {
            try
            {
                serializerSettings = serializerSettings ?? new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.None
                };

                return JsonConvert.SerializeObject(input, serializerSettings);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T FromJson<T>(this string input, JsonSerializerSettings serializerSettings = null)
        {
            try
            {
                serializerSettings = serializerSettings ?? new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Newtonsoft.Json.Formatting.None
                };

                return JsonConvert.DeserializeObject<T>(input, serializerSettings);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}

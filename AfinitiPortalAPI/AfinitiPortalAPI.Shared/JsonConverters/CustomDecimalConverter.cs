using AfinitiPortalAPI.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AfinitiPortalAPI.Shared.JsonConverters
{
    public class CustomDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return decimal.Parse(reader.GetString(), Thread.CurrentThread.CurrentCulture);
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            if (value.IsWholeNumber() && !value.ToString().Contains("."))
                writer.WriteNumberValue(decimal.Parse($"{value}.00"));
            else writer.WriteNumberValue(value);
        }
    }
}

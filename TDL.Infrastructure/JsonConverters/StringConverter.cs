using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDL.Infrastructure.Extensions;

namespace TDL.Infrastructure.JsonConverters
{
    public class StringConverter : JsonConverter<string>
    {
        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            if(value.IsValidJsonArray())
            {
                writer.WriteRawValue(value);
                return;
            }

            if(value == "null")
            {
                writer.WriteNull();

                return;
            }

            writer.WriteValue(value);
        }

        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, 
            bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => true;
        public override bool CanRead => false;
    }
}

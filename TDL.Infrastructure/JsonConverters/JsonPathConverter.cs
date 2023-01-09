using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using TDL.Infrastructure.Utilities;

namespace TDL.Infrastructure.JsonConverters
{
    public class JsonPathConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);

            var targetObj = Activator.CreateInstance(objectType);

            foreach(var prop in objectType.GetProperties()
                .Where(p => p.CanRead && p.CanWrite))
            {
                var att = prop.GetCustomAttributes(true)
                    .OfType<JsonPropertyAttribute>()
                    .FirstOrDefault();

                var jsonPath = (att != null ? att.PropertyName : prop.Name);
                var token = jObject.SelectToken(jsonPath);

                Guard.DoByCondition(token != null && token.Type != JTokenType.Null, () =>
                {
                    var value = token.ToObject(prop.PropertyType, serializer);
                    prop.SetValue(targetObj, value, null);
                });
            }

            return targetObj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

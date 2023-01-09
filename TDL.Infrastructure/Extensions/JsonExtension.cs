using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TDL.Infrastructure.Extensions
{
    public static class JsonExtension
    {
        public static T ToObject<T>(this string json)
        {
            return json.IsValidJson()
                ? JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings())
                : default;
        }

        /// <summary>
        /// Check if a string is a valid json array format.
        /// </summary>
        public static bool IsValidJsonArray(this string json)
        {
            json = json.Trim();

            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                return ValidateJson(json);
            }

            return false;
        }

        public static bool TryParse<T>(this string json, out T result)
        {
            result = default;

            if(!json.IsValidJson())
            {
                return false;
            }

            try
            {
                result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings());

                return true;
            }
            catch
            {
                // Trying to parse the result into an object.
                // If any errors occur that means it cannot be parsed, the return false.
            }

            return false;
        }

        public static bool IsValidJson(this string json)
        {
            json = json.Trim();

            if(json.StartsWith("{") && json.EndsWith("}") || 
                json.StartsWith("[") && json.EndsWith("]"))
            {
                return ValidateJson(json);
            }

            return false;
        }

        private static bool ValidateJson(string json)
        {
            try
            {
                JToken.Parse(json);
                return true;
            }
            catch(JsonReaderException)
            {
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
 
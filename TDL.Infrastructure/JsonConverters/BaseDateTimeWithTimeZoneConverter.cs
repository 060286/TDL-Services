using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NodaTime;
using System;
using TDL.Infrastructure.Extensions;

namespace TDL.Infrastructure.JsonConverters
{
    public abstract class BaseDateTimeWithTimeZoneConverter<TDateTime> : JsonConverter<TDateTime>
    {
        protected IHttpContextAccessor HttpContextAccessor;

        public override void WriteJson(JsonWriter writer, TDateTime value, JsonSerializer serializer)
        {
            var requestedTimeZone = HttpContextAccessor.HttpContext.GetTimeZone();

            if(string.IsNullOrEmpty(requestedTimeZone) || !(value is DateTime datetime))
            {
                writer.WriteValue(value);
                return;
            }

            var specifiedDateTimeUtc = DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
            var timeZone = DateTimeZoneProviders.Tzdb[requestedTimeZone.Trim()];
            var zonedDateTime = Instant.FromDateTimeUtc(specifiedDateTimeUtc).InZone(timeZone).ToDateTimeUnspecified();
            writer.WriteValue(zonedDateTime);
        }

        public override TDateTime ReadJson(JsonReader reader, Type objectType, TDateTime existingValue, bool hasExistingValue, 
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite =>
            typeof(TDateTime) == typeof(DateTime) || typeof(TDateTime) == typeof(DateTime?);

        public override bool CanRead => false;
    }
}

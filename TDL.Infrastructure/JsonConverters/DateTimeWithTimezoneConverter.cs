using Microsoft.AspNetCore.Http;
using System;

namespace TDL.Infrastructure.JsonConverters
{
    public class DateTimeWithTimezoneConverter : BaseDateTimeWithTimeZoneConverter<DateTime>
    {
        public DateTimeWithTimezoneConverter(IHttpContextAccessor httpContextAccessor) {
            HttpContextAccessor = httpContextAccessor;
        }
    }
}

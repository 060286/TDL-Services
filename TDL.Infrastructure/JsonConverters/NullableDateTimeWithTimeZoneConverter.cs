using Microsoft.AspNetCore.Http;
using System;

namespace TDL.Infrastructure.JsonConverters
{
    public class NullableDateTimeWithTimeZoneConverter : BaseDateTimeWithTimeZoneConverter<DateTime?>
    {
        public NullableDateTimeWithTimeZoneConverter(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
    }
}

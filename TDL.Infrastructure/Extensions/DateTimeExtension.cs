using System;
using System.Globalization;

namespace TDL.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ParseExact(this string dateTime, string format, IFormatProvider formatProvider = null)
        {
            return DateTime.ParseExact(dateTime, format, formatProvider ?? CultureInfo.InvariantCulture);
        }

        public static bool EqualToYear(this DateTime? datetime, int year)
        {
            return datetime != null && datetime.Value.Year == year;
        }

        public static bool EqualToMonth(this DateTime? datetime, int month)
        {
            return datetime != null && datetime.Value.Month == month;
        }
    }
}

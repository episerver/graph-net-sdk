using System;
using System.Globalization;

namespace EPiServer.ContentGraph.Helpers
{
    public static class DateExtensions
    {
        public static TimeSpan Days(this int value)
        {
            return new TimeSpan(value, 0, 0);
        }

        internal static DateTime FromUnixTime(long unixTime, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, dateTimeKind);
            return epoch.AddSeconds(unixTime).ToLocalTime();
        }

        public static DateTime NormalizeToMinutes(this DateTime date)
        {
            return date.Date.AddHours(date.Hour).AddMinutes(date.Minute);
        }

        public static DateTime? NormalizeToMinutes(this DateTime? date)
        {
            return date.HasValue ? date.Value.NormalizeToMinutes() : (DateTime?) null;
        }

        public static string ToISO8601UTCString(this DateTime date)
        {
            return date.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
        }
    }
}

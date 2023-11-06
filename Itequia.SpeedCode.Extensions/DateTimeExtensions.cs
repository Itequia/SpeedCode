using System;
using System.Globalization;

namespace Itequia.SpeedCode.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToDateTime(this object date)
        {
            if (DateTime.TryParse(date.ToStringSafe(), out var result)) return result;
            return null;
        }

        private static readonly CultureInfo ci = new CultureInfo("Es-Es");
        private static readonly TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
        public static string GetDayName(this DateTime date) => ci.DateTimeFormat.GetDayName(date.DayOfWeek);
        public static string GetMonthName(this DateTime date) => ci.DateTimeFormat.GetMonthName(date.Month);
        public static string GetMonthNameAndYear(this DateTime date) => $"{ci.DateTimeFormat.GetMonthName(date.Month)} {date.Year}";

        public static bool IsDate(this object date) => date.ToDateTime() != null;

        public static int GetRemainingMonths(this DateTime startDate)
        {
            int monthsApart = 12 * (startDate.Year - DateTime.Today.Year) + startDate.Month - DateTime.Today.Month;
            return Math.Abs(monthsApart);
        }

       

        public static DateTime? ToUtcDateTime(this object date)
        {
            if (DateTime.TryParse(date.ToStringSafe(), out var result)) return result.ToUniversalTime();
            return null;
        }

        public static string ToRentaString(this DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : string.Empty;
        }

        public static string ElapsedUntilNow(this DateTime start)
        {
            var elapsed = DateTime.Now - start;

            var result = string.Empty;

            if (Math.Floor(elapsed.TotalHours) > 0) result += Math.Floor(elapsed.TotalHours).ToString("###") + "h:";
            if (elapsed.Minutes > 0) result += elapsed.Minutes.ToString("##") + "m:";
            if (elapsed.Seconds > 0) result += elapsed.Seconds.ToString("##") + "s:";
            if (elapsed.Milliseconds > 0) result += elapsed.Milliseconds.ToString("###") + "ms";

            return result;
        }

        public static DateTime ToLocalDateTime(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(tz.GetUtcOffset(dateTime).Hours);
        }
    }
}

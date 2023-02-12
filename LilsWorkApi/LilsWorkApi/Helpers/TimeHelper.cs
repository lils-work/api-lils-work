namespace LilsWorkApi.Helpers
{
    public static class TimeHelper
    {
        public static DateTimeOffset ToZone(this DateTimeOffset time, int offsetHours)
        {
            return time.ToOffset(TimeSpan.FromHours(offsetHours));
        }

        public static DateTimeOffset ToUtc(this DateTimeOffset time)
        {
            return time.ToOffset(TimeSpan.FromHours(0));
        }

        public static DateTimeOffset? ToZone(this DateTimeOffset? time, int offsetHours)
        {
            return time == null ? null : ((DateTimeOffset)time).ToOffset(TimeSpan.FromHours(offsetHours));
        }

        public static DateTimeOffset? ToUtc(this DateTimeOffset? time)
        {
            return time == null ? null : ((DateTimeOffset)time).ToOffset(TimeSpan.FromHours(0));
        }

        public static DateTimeOffset ThisWeek(this DateTimeOffset time)
        {
            return time.Date.AddDays(-(int)time.DayOfWeek);
        }

        public static DateTimeOffset ThisMonth(this DateTimeOffset time)
        {
            return time.Date.AddDays(-time.Day + 1);
        }

        public static DateTimeOffset ThisYear(this DateTimeOffset time)
        {
            return time.Date.AddDays(-time.DayOfYear + 1);
        }
    }
}

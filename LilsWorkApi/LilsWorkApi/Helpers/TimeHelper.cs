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
    }
}

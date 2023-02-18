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

        /// <summary>
        /// ��ȡָ��ʱ������Сʱ������ʱ����
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>ָ��ʱ������Сʱ</returns>
        public static DateTimeOffset ThisHour(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Date, time.Offset).AddHours(time.Hour);
        }

        public static DateTimeOffset ThisDay(this DateTimeOffset time)
        {
            return new DateTimeOffset(time.Date, time.Offset);
        }

        public static DateTimeOffset ThisWeek(this DateTimeOffset time)
        {
            return ThisDay(time).AddDays(-(int)time.DayOfWeek);
        }

        public static DateTimeOffset ThisMonth(this DateTimeOffset time)
        {
            return ThisDay(time).AddDays(-time.Day + 1);
        }

        public static DateTimeOffset ThisYear(this DateTimeOffset time)
        {
            return ThisDay(time).AddDays(-time.DayOfYear + 1);
        }
    }
}

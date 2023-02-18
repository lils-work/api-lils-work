using Xunit;
using LilsWorkApi.Helpers;

namespace LilsWorkApi.Test.Helpers
{
    public class TimeHelper_Test
    {
        [Fact]
        public void ToZone_InputUtc0Time_ShouldGetUtc8Time()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 +00:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var utc8time = utctime.ToZone(8);
            Assert.Equal(20, utc8time.Hour);
        }

        [Fact]
        public void ToZone_InputUtc_n8Time_ShouldGetUtc8Time()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var utc8time = utctime.ToZone(8);
            Assert.Equal(12, utc8time.Day);
            Assert.Equal(4, utc8time.Hour);
        }

        [Fact]
        public void GetNextHour_InputBaseTime_ShoudGet()
        {
            var baseTimeUtc0 = DateTimeOffset.ParseExact("2023-02-11 12:13:27 +00:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisHourUtc0 = baseTimeUtc0.ThisHour();
            Assert.Equal(12, thisHourUtc0.Hour);
            Assert.Equal(0, thisHourUtc0.Offset.Hours);

            var baseTimeUtc8 = DateTimeOffset.ParseExact("2023-02-11 12:13:27 +08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisHourUtc8 = baseTimeUtc8.ThisHour();
            Assert.Equal(12, thisHourUtc8.Hour);
            Assert.Equal(4, thisHourUtc8.ToZone(0).Hour);
            Assert.Equal(8, thisHourUtc8.Offset.Hours);
        }

        [Fact]
        public void ThisDay_InputUtcTime_ShouldGet0am()
        {
            var baseTimeUtc0 = DateTimeOffset.ParseExact("2023-02-11 12:13:27 +00:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisDayUtc0 = baseTimeUtc0.ThisDay();
            Assert.Equal(0, thisDayUtc0.Hour);
            Assert.Equal(0, thisDayUtc0.Offset.Hours);

            var baseTimeUtc8 = DateTimeOffset.ParseExact("2023-02-11 12:13:27 +08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisDayUtc8 = baseTimeUtc8.ThisDay();
            Assert.Equal(0, thisDayUtc8.Hour);
            Assert.Equal(8, thisDayUtc8.Offset.Hours);

            for (int i = 0; i < 8; i++)
            {
                var hour = i * 3;
                var baseTimeUtc8_hour = DateTimeOffset.ParseExact($"2023-02-11 {hour:D2}:13:27 +08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
                var thisDayUtc8_hour = baseTimeUtc8_hour.ThisDay();
                Assert.Equal(0, thisDayUtc8_hour.Hour);
                Assert.Equal(8, thisDayUtc8_hour.Offset.Hours);
            }
        }

        [Fact]
        public void ThisWeek_InputUtcTime_ShouldGetSunday()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisWeek = utctime.ThisWeek();
            Assert.Equal(5, thisWeek.Day);
            Assert.Equal(0, thisWeek.Hour);
            Assert.Equal(-8, thisWeek.Offset.Hours);
        }

        [Fact]
        public void ThisMonth_InputUtcTime_ShouldGet1st()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisMonth = utctime.ThisMonth();
            Assert.Equal(1, thisMonth.Day);
            Assert.Equal(0, thisMonth.Hour);
            Assert.Equal(-8, thisMonth.Offset.Hours);
        }

        [Fact]
        public void ThisYear_InputUtcTime_ShouldGetJan1st()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisYear = utctime.ThisYear();
            Assert.Equal(1, thisYear.Month);
            Assert.Equal(1, thisYear.Day);
            Assert.Equal(0, thisYear.Hour);
            Assert.Equal(-8, thisYear.Offset.Hours);
        }
    }

    public class TimeHelper_Verify
    {
        [Fact]
        public void DateTimeOffset_Date_ShouldKeepOffset()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var utc8time = utctime.ToZone(8);
            Assert.Equal(12, utc8time.Date.Day);
        }
    }
}

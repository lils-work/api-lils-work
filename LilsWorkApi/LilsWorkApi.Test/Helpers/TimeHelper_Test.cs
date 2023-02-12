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
        public void ThisWeek_InputUtcTime_ShouldGetSunday()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisWeek = utctime.ThisWeek();
            Assert.Equal(5, thisWeek.Day);
            Assert.Equal(0, thisWeek.Hour);
        }

        [Fact]
        public void ThisMonth_InputUtcTime_ShouldGet1st()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisMonth = utctime.ThisMonth();
            Assert.Equal(1, thisMonth.Day);
            Assert.Equal(0, thisMonth.Hour);
        }

        [Fact]
        public void ThisYear_InputUtcTime_ShouldGetJan1st()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var thisYear = utctime.ThisYear();
            Assert.Equal(1, thisYear.Month);
            Assert.Equal(1, thisYear.Day);
            Assert.Equal(0, thisYear.Hour);
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

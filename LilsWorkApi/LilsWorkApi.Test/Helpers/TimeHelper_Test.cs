using Xunit;
using LilsWorkApi.Helpers;
using System.Globalization;

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
        public void DateTimeOffset_Date_ShouldKeepOffset()
        {
            var utctime = DateTimeOffset.ParseExact("2023-02-11 12:13:27 -08:00", "yyyy-MM-dd HH:mm:ss zzz", null);
            var utc8time = utctime.ToZone(8);
            Assert.Equal(12, utc8time.Date.Day);
        }
    }
}

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace LilsWorkApi.Controllers
{
    [ApiController]
    [EnableCors("allowpaired")]
    [Route("[controller]")]
    public class TimeController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            var message = "";
            message += $"          SERVER NOW: {DateTime.Now}\n";
            message += $"             UTC NOW: {DateTime.UtcNow}\n";
            message += "\n";
            var timezone = 8;
            var timezoneOffset = TimeSpan.FromHours(timezone); 
            message += $"           TIME ZONE: {(timezone > 0 ? "+" : "")}{timezone}\n";
            message += $"   OFFSET SERVER NOW: {DateTimeOffset.Now}\n";
            message += $"      OFFSET UTC NOW: {DateTimeOffset.UtcNow}\n";
            message += $"     OFFSET ZONE NOW: {DateTimeOffset.UtcNow.ToOffset(timezoneOffset)}\n";
            message += $"   OFFSET ZONE TODAY: {DateTimeOffset.UtcNow.ToOffset(timezoneOffset).Date}\n";
            message += $"OFFSET ZONE TOMORROW: {DateTimeOffset.UtcNow.ToOffset(timezoneOffset).Date.AddDays(1)}\n";

            return await Task.FromResult(message);
        }
    }
}
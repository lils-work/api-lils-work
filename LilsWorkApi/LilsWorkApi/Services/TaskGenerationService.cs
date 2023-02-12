using LilsWorkApi.Helpers;

namespace LilsWorkApi.Services
{
    /// <summary>
    /// 任务生成服务；
    /// 代码参考 https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio#backgroundservice-base-class
    /// </summary>
    public class TaskGenerationService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private Timer? timer;

        public TaskGenerationService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(CheckGeneration, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void CheckGeneration(object? state)
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

            // 每小时生成的周期性任务 - UTC+8 时间
            var utc8now = DateTimeOffset.UtcNow.ToZone(TimeZoneHelper.CurrentTimeZone);
            var utc8thishour = utc8now.Date.AddHours(utc8now.Hour);
            // 找到还没有创建的计划
            var hourlyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Hourly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thishour));
            dbContext.Tasks.AddRange(hourlyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thishour.AddHours(1),
            }));

            // TODO 后续任务考虑使用更低频的计时器，或专门提供周期性执行任务的辅助类

            // 每天生成的周期性任务 - UTC+8 时间
            var utc8today = DateTimeOffset.UtcNow.ToZone(TimeZoneHelper.CurrentTimeZone).Date;
            // 找到还没有创建的计划
            var dailyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Daily)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8today));
            dbContext.Tasks.AddRange(dailyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8today.AddDays(1),
            }));

            // 每周生成的周期性任务 - UTC+8 时间
            var utc8thisweek = utc8now.ThisWeek();
            // 找到还没有创建的计划
            var weeklyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Weekly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thisweek));
            dbContext.Tasks.AddRange(weeklyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thisweek.AddDays(7),
            }));

            // 每月生成的周期性任务 - UTC+8 时间
            var utc8thismonth = utc8now.ThisMonth();
            // 找到还没有创建的计划
            var monthlyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Monthly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thismonth));
            dbContext.Tasks.AddRange(monthlyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thismonth.AddMonths(1),
            }));

            // 每年生成的周期性任务 - UTC+8 时间
            var utc8thisyear = utc8now.ThisYear();
            // 找到还没有创建的计划
            var yearlyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Yearly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thisyear));
            dbContext.Tasks.AddRange(yearlyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thisyear.AddYears(1),
            }));

            await dbContext.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
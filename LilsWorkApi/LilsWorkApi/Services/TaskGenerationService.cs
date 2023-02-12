using LilsWorkApi.Helpers;

namespace LilsWorkApi.Services
{
    /// <summary>
    /// �������ɷ���
    /// ����ο� https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio#backgroundservice-base-class
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

            // ÿСʱ���ɵ����������� - UTC+8 ʱ��
            var utc8now = DateTimeOffset.UtcNow.ToZone(TimeZoneHelper.CurrentTimeZone);
            var utc8thishour = utc8now.Date.AddHours(utc8now.Hour);
            // �ҵ���û�д����ļƻ�
            var hourlyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Hourly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thishour));
            dbContext.Tasks.AddRange(hourlyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thishour.AddHours(1),
            }));

            // TODO ����������ʹ�ø���Ƶ�ļ�ʱ������ר���ṩ������ִ������ĸ�����

            // ÿ�����ɵ����������� - UTC+8 ʱ��
            var utc8today = DateTimeOffset.UtcNow.ToZone(TimeZoneHelper.CurrentTimeZone).Date;
            // �ҵ���û�д����ļƻ�
            var dailyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Daily)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8today));
            dbContext.Tasks.AddRange(dailyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8today.AddDays(1),
            }));

            // ÿ�����ɵ����������� - UTC+8 ʱ��
            var utc8thisweek = utc8now.ThisWeek();
            // �ҵ���û�д����ļƻ�
            var weeklyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Weekly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thisweek));
            dbContext.Tasks.AddRange(weeklyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thisweek.AddDays(7),
            }));

            // ÿ�����ɵ����������� - UTC+8 ʱ��
            var utc8thismonth = utc8now.ThisMonth();
            // �ҵ���û�д����ļƻ�
            var monthlyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Monthly)
                .Where(tp => !dbContext.Tasks.Any(t => t.Title == tp.Title && t.DueTo >= utc8thismonth));
            dbContext.Tasks.AddRange(monthlyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = utc8thismonth.AddMonths(1),
            }));

            // ÿ�����ɵ����������� - UTC+8 ʱ��
            var utc8thisyear = utc8now.ThisYear();
            // �ҵ���û�д����ļƻ�
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
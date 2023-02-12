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

            // �������Ϊ�յ�����
            var today = DateTime.Today;
            var dueToday = today.AddDays(1).AddSeconds(-1);
            var dailyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Daily)
                .Where(tp => !dbContext.Tasks.Any(t => t.DueTo > today && t.Title == tp.Title));

            dbContext.Tasks.AddRange(dailyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                CreatedAt = DateTime.Now,
                DueTo = dueToday,
            }));

            // TEST ÿСʱ���ɵ����������� - UTC+8 ʱ��
            var utc8now = DateTimeOffset.UtcNow.ToZone(+8);
            var utc8thishour = utc8now.Date.AddHours(utc8now.Hour);
            if (!dbContext.Tasks.Any(t => t.CreatedAt >= utc8thishour && t.Title != null && t.Title.StartsWith("HOUR TASK")))
            {
                // ��ǰСʱû������ʱ������������
                dbContext.Tasks.Add(new Models.Task
                {
                    Title = $"HOUR TASK {utc8thishour:MM.dd HH:mm:ss zzz}",
                    CreatedAt = DateTime.Now,
                    DueTo = utc8thishour.AddHours(1),
                });
            }

            // TEST ÿ�����ɵ����������� - UTC+8 ʱ��
            var utc8today = DateTimeOffset.UtcNow.ToZone(+8).Date;
            if (!dbContext.Tasks.Any(t => t.CreatedAt >= utc8today && t.Title != null && t.Title.StartsWith("DAILY TASK")))
            {
                // ����û������ʱ������������
                dbContext.Tasks.Add(new Models.Task
                {
                    Title = $"DAILY TASK {utc8today:MM.dd HH:mm:ss zzz}",
                    CreatedAt = DateTime.Now,
                    DueTo = utc8today.AddDays(1),
                });
            }

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